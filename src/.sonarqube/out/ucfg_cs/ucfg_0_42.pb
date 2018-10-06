
[Mix.Common.Helper.CommonHelper.UploadFileAsync(string, Microsoft.AspNetCore.Http.IFormFile)^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csï (	fullPathfile"0*
0*
1
2*Â
2´
±
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csó ó(/
%0""System.IO.Directory.Exists(string)*"
System.IO.Directory*


fullPath*
3
4*È
3½
º
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csõ õ(7
%1"+System.IO.Directory.CreateDirectory(string)*"
System.IO.Directory*


fullPath*
4*¹
4«
¨
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csø ø( 
%2""object.operator !=(object, object)*
"
object*

file*
""*
5
6*ô
5‘

^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csı ı(&
%3"System.Guid.NewGuid()*"
System.Guid—
”
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csı ı(4
%4"System.Guid.ToString(string)*

%3*
""¥
¢
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csş ş(%
%5"0Microsoft.AspNetCore.Http.IFormFile.FileName.get*

file–
“
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csş ş(0
%6"string.Split(params char[])*

%5*
""ä
á
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csş ş(7
%7"USystem.Linq.Enumerable.Last<TSource>(System.Collections.Generic.IEnumerable<TSource>)*"
System.Linq.Enumerable*

%6´
±
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csü& ş(8
%8"%string.Format(string, object, object)*
"
string*
""*

%4*

%7|
z
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csü ş(8

fileName"__id*

%8Á
¾
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csÿ; ÿ([
%9"&System.IO.Path.Combine(string, string)*"
System.IO.Path*


fullPath*


fileName”
‘
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csÿ] ÿ(l
%10"__id*"* 
System.IO.FileMode"
Createš
—
_
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csÿn ÿ(‚
%11"__id*'*%
System.IO.FileAccess"
	ReadWrite€~
_
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csÿ, ÿ(ƒ
%12"System.IO.FileStreamà
İ
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csÿ0 ÿ(:
%13"QSystem.IO.FileStream.FileStream(string, System.IO.FileMode, System.IO.FileAccess)*

%12*

%9*

%10*

%11€
~
_
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csÿ ÿ(ƒ

fileStream"__id*

%12*
7*™
7ó
ğ
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.cs (:
%14"eMicrosoft.AspNetCore.Http.IFormFile.CopyToAsync(System.IO.Stream, System.Threading.CancellationToken)*

file*


fileStream*
""­
ª
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.cs (P
%15"0System.Threading.Tasks.Task.ConfigureAwait(bool)*

%14*
"""n
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.cs‚ ‚((


fileName*
8*
9*ø
6‡
„
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.cs‡ ‡('
%16"__id**
string"
Empty"i
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.cs‡ ‡((

%16*
9*
1
10*ø
1‡
„
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csŒ Œ(#
%17"__id**
string"
Empty"i
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csŒ Œ($

%17*
10"
""