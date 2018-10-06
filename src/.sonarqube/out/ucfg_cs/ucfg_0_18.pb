
MMix.Common.Helper.UnitOfWorkHelper<TDbContext>.LogException(System.Exception)`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.csV ~(	ex"0*’
0¬
©
`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.csX/ X(K
%0"'System.Environment.CurrentDirectory.get*"
System.Environment¯
¬
`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.csX X(S
%1"&string.Format(string, params object[])*
"
string*
""*
""~
|
`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.csX X(S

fullPath"__id*

%1£
 
`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.csY Y(/
%2"string.IsNullOrEmpty(string)*
"
string*


fullPath*
1
2*Ä
1¶
³
`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.csY4 Y(N
%3""System.IO.Directory.Exists(string)*"
System.IO.Directory*


fullPath*
3
2*Ê
3¿
¼
`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.cs[ [(3
%4"+System.IO.Directory.CreateDirectory(string)*"
System.IO.Directory*


fullPath*
2*‹
2~
|
`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.cs] ](>

filePath"__id*
""*
4
5*Æ
5~|
`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.csa  a(6
%5"System.IO.FileInfo¦
£
`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.csa$ a(,
%6"#System.IO.FileInfo.FileInfo(string)*

%5*


filePathz
x
`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.csa a(6
file"__id*

%5}
{
`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.csb b(%	
content"__id*
""”
‘
`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.csc c(
%7"System.IO.FileInfo.Exists.get*

file*
6
7*˜
6”
‘
`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.cse, e(;
%8"System.IO.FileInfo.OpenText()*

filew
u
`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.cse( e(;
s"__id*

%8*
8* 
8–
“
`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.csg" g(/
%9""System.IO.StreamReader.ReadToEnd()*

s}
{
`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.csg g(/	
content"__id*

%9*
9*¸
9­
ª
`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.csi i()
%10"System.IO.File.Delete(string)*"
System.IO.File*


filePath*
7*¶
7Å
Â
`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.csl' l(<
%11")Newtonsoft.Json.Linq.JArray.Parse(string)*"
Newtonsoft.Json.Linq.JArray*
	
content…
‚
`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.csl l(<
arrExceptions"__id*

%11Š‡
`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.csm q(
%12"Newtonsoft.Json.Linq.JObject
š
`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.csm" m()
%13"&Newtonsoft.Json.Linq.JObject.JObject()*

%12
š
`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.cso5 o(D
%14"System.DateTime.UtcNow.get*"
System.DateTimeŒ‰
`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.cso o(E
%15"Newtonsoft.Json.Linq.JPropertyÀ
½
`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.cso o(!
%16"8Newtonsoft.Json.Linq.JProperty.JProperty(string, object)*

%15*
""*

%14Ç
Ä
`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.csp- p(C
%17"/Newtonsoft.Json.Linq.JObject.FromObject(object)* "
Newtonsoft.Json.Linq.JObject*

exŒ‰
`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.csp p(D
%18"Newtonsoft.Json.Linq.JPropertyÀ
½
`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.csp p(!
%19"8Newtonsoft.Json.Linq.JProperty.JProperty(string, object)*

%18*
""*

%17z
x
`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.csm q(
jex"__id*

%12Æ
Ã
`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.csr r(&
%20"<Newtonsoft.Json.Linq.JArray.Add(Newtonsoft.Json.Linq.JToken)*

arrExceptions*

jex§
¤
`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.css s(2
%21"&Newtonsoft.Json.Linq.JToken.ToString()*

arrExceptions~
|
`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.css s(2	
content"__id*

%21±
®
`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.csu$ u(=
%22"!System.IO.File.CreateText(string)*"
System.IO.File*


filePath}
{
`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.csu u(=
writer"__id*

%22*
10*º
10­
ª
`
VC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\UnitOfWorkHelper.csw w(-
%23"&System.IO.TextWriter.WriteLine(string)*


writer*
	
content*
11*
11*
4
4*
4"
""