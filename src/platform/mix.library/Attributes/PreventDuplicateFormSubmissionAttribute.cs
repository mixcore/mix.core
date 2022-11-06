namespace Mix.Lib.Attributes
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Mix.Lib.Extensions;
    using System;
    using System.Threading.Tasks;

    // This class provides an attribute for controller actions that flags duplicate form submissions
    // by adding a model error if the request's verification token has already been seen on a prior
    // form submission.
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class PreventDuplicateFormSubmissionAttribute : ActionFilterAttribute
    {
        const string TokenKey = "__RequestVerificationToken";
        const string HistoryKey = "RequestVerificationTokenHistory";
        const int HistoryCapacity = 5;

        const string DuplicateSubmissionErrorMessage =
            "Your request was received more than once (either due to a temporary problem with the network or a " +
            "double button press). Any submissions after the first one have been rejected, but the status of the " +
            "first one is unclear. It may or may not have succeeded. Please check elsewhere to verify that your " +
            "request had the intended effect. You may need to resubmit it.";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            HttpRequest request = context.HttpContext.Request;

            if (request.HasFormContentType && request.Form.ContainsKey(TokenKey))
            {
                string token = request.Form[TokenKey].ToString();

                ISession session = context.HttpContext.Session;
                var history = session.Get<RotatingHistory<string>>(HistoryKey) ?? new RotatingHistory<string>(HistoryCapacity);

                if (history.Contains(token))
                {
                    context.ModelState.AddModelError("", DuplicateSubmissionErrorMessage);
                }
                else
                {
                    history.Add(token);
                    session.Put(HistoryKey, history);
                }
            }
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            HttpRequest request = context.HttpContext.Request;
            ISession session = context.HttpContext.Session;
            await session.LoadAsync();
            var history = session.Get<RotatingHistory<string>>(HistoryKey) ?? new RotatingHistory<string>(HistoryCapacity);
            string token = $"{request.Path}-{request.Method}";
            if (history.Contains(token))
            {
                context.ModelState.AddModelError("", DuplicateSubmissionErrorMessage);
                throw new MixException(DuplicateSubmissionErrorMessage);
            }
            else
            {
                history.Add(token);
                session.Put(HistoryKey, history);
                await session.CommitAsync();
            }
            await next();
        }
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            context.HttpContext.Session.Remove(HistoryKey);
            _ = context.HttpContext.Session.CommitAsync();
            base.OnResultExecuted(context);
        }
    }

    // This class stores the last x items in an array.  Adding a new item overwrites the oldest item
    // if there is no more empty space.  For the purpose of being JSON-serializable, its data is
    // stored via public properties and it has a parameterless constructor.
    public class RotatingHistory<T>
    {
        public T[] Items { get; set; }
        public int Index { get; set; }

        public RotatingHistory() { }

        public RotatingHistory(int capacity)
        {
            Items = new T[capacity];
        }

        public void Add(T item)
        {
            Items[Index] = item;
            Index = ++Index % Items.Length;
        }

        public bool Contains(T item)
        {
            return Items.Contains(item);
        }
    }
}
