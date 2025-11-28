namespace Mix.Lib.Dtos
{
    public class UpdatePriorityDto<TPrimaryKey>
    {
        public TPrimaryKey Id { get; set; }
        public int Priority { get; set; }
    }
}
