
YRewriteRules.MethodRules.RedirectXMLRequests(Microsoft.AspNetCore.Rewrite.RewriteContext)O
EC:\_workspace\github\mixcore\mix.core\src\Mix.Cms.Web\RewriteRules.cs (	context"0*≈
0§
°
O
EC:\_workspace\github\mixcore\mix.core\src\Mix.Cms.Web\RewriteRules.cs (-
%0";Microsoft.AspNetCore.Rewrite.RewriteContext.HttpContext.get*
	
contextï
í
O
EC:\_workspace\github\mixcore\mix.core\src\Mix.Cms.Web\RewriteRules.cs (5
%1"1Microsoft.AspNetCore.Http.HttpContext.Request.get*

%0l
j
O
EC:\_workspace\github\mixcore\mix.core\src\Mix.Cms.Web\RewriteRules.cs (5	
request"__id*

%1ó
î
O
EC:\_workspace\github\mixcore\mix.core\src\Mix.Cms.Web\RewriteRules.cs (
%2".Microsoft.AspNetCore.Http.HttpRequest.Path.get*
	
request}
O
EC:\_workspace\github\mixcore\mix.core\src\Mix.Cms.Web\RewriteRules.cs0 (K
%3"$Microsoft.AspNetCore.Http.PathString£
†
O
EC:\_workspace\github\mixcore\mix.core\src\Mix.Cms.Web\RewriteRules.cs4 (>
%4"7Microsoft.AspNetCore.Http.PathString.PathString(string)*

%3*
""…
∆
O
EC:\_workspace\github\mixcore\mix.core\src\Mix.Cms.Web\RewriteRules.cs (L
%5"]Microsoft.AspNetCore.Http.PathString.StartsWithSegments(Microsoft.AspNetCore.Http.PathString)*

%2*

%3*
1
2*^
1"Y
O
EC:\_workspace\github\mixcore\mix.core\src\Mix.Cms.Web\RewriteRules.cs (
""*¯
2ó
î
O
EC:\_workspace\github\mixcore\mix.core\src\Mix.Cms.Web\RewriteRules.cs (
%6".Microsoft.AspNetCore.Http.HttpRequest.Path.get*
	
requestí
è
O
EC:\_workspace\github\mixcore\mix.core\src\Mix.Cms.Web\RewriteRules.cs ("
%7".Microsoft.AspNetCore.Http.PathString.Value.get*

%6î
ë
O
EC:\_workspace\github\mixcore\mix.core\src\Mix.Cms.Web\RewriteRules.cs4 (V
%8"__id*2*0
System.StringComparison"
OrdinalIgnoreCase§
°
O
EC:\_workspace\github\mixcore\mix.core\src\Mix.Cms.Web\RewriteRules.cs (W
%9"0string.EndsWith(string, System.StringComparison)*

%7*
""*

%8*
3
4*¸
3•
¢
O
EC:\_workspace\github\mixcore\mix.core\src\Mix.Cms.Web\RewriteRules.cs (2
%10";Microsoft.AspNetCore.Rewrite.RewriteContext.HttpContext.get*
	
contextò
ï
O
EC:\_workspace\github\mixcore\mix.core\src\Mix.Cms.Web\RewriteRules.cs (;
%11"2Microsoft.AspNetCore.Http.HttpContext.Response.get*

%10n
l
O
EC:\_workspace\github\mixcore\mix.core\src\Mix.Cms.Web\RewriteRules.cs (;

response"__id*

%11´
®
O
EC:\_workspace\github\mixcore\mix.core\src\Mix.Cms.Web\RewriteRules.cs& (K
%12"__id*H*F'
%Microsoft.AspNetCore.Http.StatusCodes"
Status301MovedPermanently©
¶
O
EC:\_workspace\github\mixcore\mix.core\src\Mix.Cms.Web\RewriteRules.cs (K
%13"5Microsoft.AspNetCore.Http.HttpResponse.StatusCode.set*


response*

%12ü
ú
O
EC:\_workspace\github\mixcore\mix.core\src\Mix.Cms.Web\RewriteRules.cs! (7
%14"__id*<*:)
'Microsoft.AspNetCore.Rewrite.RuleResult"
EndResponse©
¶
O
EC:\_workspace\github\mixcore\mix.core\src\Mix.Cms.Web\RewriteRules.cs (7
%15"6Microsoft.AspNetCore.Rewrite.RewriteContext.Result.set*
	
context*

%14ù
ö
O
EC:\_workspace\github\mixcore\mix.core\src\Mix.Cms.Web\RewriteRules.cs ( 
%16"2Microsoft.AspNetCore.Http.HttpResponse.Headers.get*


responseõ
ò
O
EC:\_workspace\github\mixcore\mix.core\src\Mix.Cms.Web\RewriteRules.cs! (5
%17"__id*8*6(
&Microsoft.Net.Http.Headers.HeaderNames"

Locationò
ï
O
EC:\_workspace\github\mixcore\mix.core\src\Mix.Cms.Web\RewriteRules.cs" (.
%18".Microsoft.AspNetCore.Http.HttpRequest.Path.get*
	
requestı
Ú
O
EC:\_workspace\github\mixcore\mix.core\src\Mix.Cms.Web\RewriteRules.cs (.
%19"]Microsoft.AspNetCore.Http.PathString.operator +(string, Microsoft.AspNetCore.Http.PathString)*("&
$Microsoft.AspNetCore.Http.PathString*
""*

%18ü
ú
O
EC:\_workspace\github\mixcore\mix.core\src\Mix.Cms.Web\RewriteRules.cs1 (D
%20"5Microsoft.AspNetCore.Http.HttpRequest.QueryString.get*
	
requestv
t
O
EC:\_workspace\github\mixcore\mix.core\src\Mix.Cms.Web\RewriteRules.cs (D
%21"__concat*

%19*

%20´
®
O
EC:\_workspace\github\mixcore\mix.core\src\Mix.Cms.Web\RewriteRules.cs (D
%22"<Microsoft.AspNetCore.Http.IHeaderDictionary.this[string].set*

%16*

%17*
4*
4"
""