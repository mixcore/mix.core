
?Mix.Common.Helper.CommonHelper.WriteBytesToFile(string, string)^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csï ©(	fullPath	strBase64"0*õ
0ñ
ì
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csó2 ó(H
%0"string.IndexOf(char)*

	strBase64*
""ó
î
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csó ó(M
%1"string.Substring(int)*

	strBase64*
""|
z
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csó ó(M

fileData"__id*

%1¥
±
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csò ò(=
%2"'System.Convert.FromBase64String(string)*"
System.Convert*


fileDatay
w
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csò ò(=
bytes"__id*

%2™
ß
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csô ô(%
%3"System.IO.File.Exists(string)*"
System.IO.File*


fullPath*
1
2*µ
1™
ß
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csõ õ(%
%4"System.IO.File.Delete(string)*"
System.IO.File*


fullPath*
2*ç
2ì
ê
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csû5 û(D
%5"__id*"* 
System.IO.FileMode"
Create~|
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csû û(E
%6"System.IO.FileStreamƒ
¡
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csû  û(*
%7";System.IO.FileStream.FileStream(string, System.IO.FileMode)*

%6*


fullPath*

%5v
t
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csû û(E
fs"__id*

%6Ä~
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csü ü(1
%8"System.IO.BinaryWriter∞
≠
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csü! ü(-
%9"5System.IO.BinaryWriter.BinaryWriter(System.IO.Stream)*

%8*

fsu
s
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csü ü(1
w"__id*

%8*
3
4*∞
3¢
ü
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.cs¢ ¢(
%10"$System.IO.BinaryWriter.Write(byte[])*

w*	

bytes*
5
4*´
4å
â
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.cs¶ ¶(
%11"System.IO.Stream.Close()*

fsë
é
^
RC:\_workspace\github\mixcore\mix.heart\src\Mix.Heart\Common\Helper\CommonHelper.csß ß(
%12"System.IO.BinaryWriter.Close()*

w*
6*
5*
6*
6"
""