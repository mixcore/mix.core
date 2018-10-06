¸
eC:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Authorization\ClaimRequirement.cs
	namespace 	
Mix
 
. 
Identity 
. 
Authorization $
{ 
public		 

class		 
ClaimRequirement		 !
:		" #%
IAuthorizationRequirement		$ =
{

 
public 
ClaimRequirement 
(  
string  &
	claimName' 0
,0 1
string2 8

claimValue9 C
)C D
{ 	
	ClaimName 
= 
	claimName !
;! "

ClaimValue 
= 

claimValue #
;# $
} 	
public 
string 
	ClaimName 
{  !
get" %
;% &
set' *
;* +
}, -
public$$ 
string$$ 

ClaimValue$$  
{$$! "
get$$# &
;$$& '
set$$( +
;$$+ ,
}$$- .
}%% 
}&& ¢
mC:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Authorization\ClaimsRequirementHandler.cs
	namespace		 	
Mix		
 
.		 
Identity		 
.		 
Authorization		 $
{

 
public 

class $
ClaimsRequirementHandler )
:* + 
AuthorizationHandler, @
<@ A
ClaimRequirementA Q
>Q R
{ 
	protected 
override 
Task "
HandleRequirementAsync  6
(6 7'
AuthorizationHandlerContext7 R
contextS Z
,Z [
ClaimRequirement? O
requirementP [
)[ \
{ 	
var 
claim 
= 
context 
.  
User  $
.$ %
Claims% +
.+ ,
FirstOrDefault, :
(: ;
c; <
=>= ?
c@ A
.A B
TypeB F
==G I
requirementJ U
.U V
	ClaimNameV _
)_ `
;` a
if 
( 
claim 
!= 
null 
&&  
claim! &
.& '
Value' ,
., -
Contains- 5
(5 6
requirement6 A
.A B

ClaimValueB L
)L M
)M N
{ 
context 
. 
Succeed 
(  
requirement  +
)+ ,
;, -
} 
return 
Task 
. 
CompletedTask %
;% &
} 	
} 
} ¤
LC:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Const.cs
	namespace 	
Mix
 
. 
Identity 
{ 
public 

class 
Const 
{ 
public 
const 
string $
CONST_DEFAULT_CONNECTION 4
=5 6
$str7 J
;J K
public 
const 
string !
CONST_FILE_APPSETTING 1
=2 3
$str4 F
;F G
public 
enum 
ApplicationTypes $
{ 	

JavaScript 
= 
$num 
, 
NativeConfidential 
=  
$num! "
} 	
;	 

} 
} ì
`C:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Data\ApplicationDbContext.cs
	namespace 	
Mix
 
. 
Identity 
. 
Data 
{ 
public 

class  
ApplicationDbContext %
:& '
IdentityDbContext( 9
<9 :
ApplicationUser: I
>I J
{ 
public  
ApplicationDbContext #
(# $
DbContextOptions$ 4
<4 5 
ApplicationDbContext5 I
>I J
optionsK R
)R S
: 
base 
( 
options "
)" #
{ 	
} 	
public  
ApplicationDbContext #
(# $
)$ %
{ 	
} 	
public   
DbSet   
<   
Client   
>   
Clients   $
{  % &
get  ' *
;  * +
set  , /
;  / 0
}  1 2
public!! 
DbSet!! 
<!! 
RefreshToken!! !
>!!! "
RefreshTokens!!# 0
{!!1 2
get!!3 6
;!!6 7
set!!8 ;
;!!; <
}!!= >
	protected77 
override77 
void77 
OnModelCreating77  /
(77/ 0
ModelBuilder770 <
builder77= D
)77D E
{88 	
base99 
.99 
OnModelCreating99  
(99  !
builder99! (
)99( )
;99) *
}== 	
	protected>> 
override>> 
void>> 
OnConfiguring>>  -
(>>- .#
DbContextOptionsBuilder>>. E
optionsBuilder>>F T
)>>T U
{?? 	
var@@ 
config@@ 
=@@ 
new@@  
ConfigurationBuilder@@ 1
(@@1 2
)@@2 3
.AA 
SetBasePathAA 
(AA 
	DirectoryAA (
.AA( )
GetCurrentDirectoryAA) <
(AA< =
)AA= >
)AA> ?
.BB 
AddJsonFileBB 
(BB 
ConstBB $
.BB$ %!
CONST_FILE_APPSETTINGBB% :
)BB: ;
.CC 
BuildCC 
(CC 
)CC 
;CC 
stringDD 
cnnDD 
=DD 
configDD 
.DD  
GetConnectionStringDD  3
(DD3 4
ConstDD4 9
.DD9 :$
CONST_DEFAULT_CONNECTIONDD: R
)DDR S
;DDS T
ifEE 
(EE 
!EE 
stringEE 
.EE 
IsNullOrEmptyEE %
(EE% &
cnnEE& )
)EE) *
)EE* +
{FF 
optionsBuilderHH 
.HH 
UseSqlServerHH +
(HH+ ,
cnnHH, /
)HH/ 0
;HH0 1
baseII 
.II 
OnConfiguringII "
(II" #
optionsBuilderII# 1
)II1 2
;II2 3
}JJ 
}LL 	
}MM 
}NN œ
UC:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Data\ClaimData.cs
	namespace 	
Mix
 
. 
Identity 
. 
Data 
{ 
public		 

static		 
class		 
IdentityBasedData		 )
{

 
public 
static 
List 
< 
string !
>! "

UserClaims# -
{. /
get0 3
;3 4
set5 8
;8 9
}: ;
=< =
new> A
ListB F
<F G
stringG M
>M N
{< =
$str@ J
,J K
$str@ K
,K L
$str@ M
}< =
;= >
} 
} ï
VC:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Entities\Client.cs
	namespace 	
Mix
 
. 
Identity 
. 
Entities 
{		 
public

 

class

 
Client

 
{ 
[ 	
Key	 
] 
public 
string 
Id 
{ 
get 
; 
set  #
;# $
}% &
[ 	
Required	 
] 
public 
string 
Secret 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	
Required	 
] 
[ 	
	MaxLength	 
( 
$num 
) 
] 
public 
string 
Name 
{ 
get  
;  !
set" %
;% &
}' (
public 
ApplicationTypes 
ApplicationType  /
{0 1
get2 5
;5 6
set7 :
;: ;
}< =
public 
bool 
Active 
{ 
get  
;  !
set" %
;% &
}' (
public 
int  
RefreshTokenLifeTime '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
[ 	
	MaxLength	 
( 
$num 
) 
] 
public 
string 
AllowedOrigin #
{$ %
get& )
;) *
set+ .
;. /
}0 1
} 
} ´	
\C:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Entities\RefreshToken.cs
	namespace 	
Mix
 
. 
Identity 
. 
Entities 
{		 
public

 

class

 
RefreshToken

 
{ 
[ 	
Key	 
] 
public 
string 
Id 
{ 
get 
; 
set  #
;# $
}% &
public 
string 
ClientId 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
DateTime 
	IssuedUtc !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 
DateTime 

ExpiresUtc "
{# $
get% (
;( )
set* -
;- .
}/ 0
[ 	
Required	 
] 
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
} 
} ´	
gC:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Extensions\EmailSenderExtensions.cs
	namespace		 	
Mix		
 
.		 
Identity		 
.		 
Services		 
{

 
public 

static 
class !
EmailSenderExtensions -
{ 
public 
static 
Task &
SendEmailConfirmationAsync 5
(5 6
this6 :
IEmailSender; G
emailSenderH S
,S T
stringU [
email\ a
,a b
stringc i
linkj n
)n o
{ 	
return 
emailSender 
. 
SendEmailAsync -
(- .
email. 3
,3 4
$str5 I
,I J
$" H
<Please confirm your account by clicking this link: <a href=' N
{N O
HtmlEncoderO Z
.Z [
Default[ b
.b c
Encodec i
(i j
linkj n
)n o
}o p

'>link</a>p z
"z {
){ |
;| }
} 	
} 
} ·
lC:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Infrastructure\ApplicationUserManager.cs
	namespace 	
Mix
 
. 
Identity 
. 
Infrastructure %
{ 
public 

class "
ApplicationUserManager '
:( )
UserManager* 5
<5 6
ApplicationUser6 E
>E F
{ 
public "
ApplicationUserManager %
(% &

IUserStore& 0
<0 1
ApplicationUser1 @
>@ A
storeB G
,G H
IOptionsI Q
<Q R
IdentityOptionsR a
>a b
optionsAccessorc r
,r s
IPasswordHasher 
< 
ApplicationUser +
>+ ,
passwordHasher- ;
,; <
IEnumerable= H
<H I
IUserValidatorI W
<W X
ApplicationUserX g
>g h
>h i
userValidatorsj x
,x y
IEnumerable 
< 
IPasswordValidator *
<* +
ApplicationUser+ :
>: ;
>; <
passwordValidators= O
,O P
ILookupNormalizerQ b
keyNormalizerc p
,p q"
IdentityErrorDescriber "
errors# )
,) *
IServiceProvider+ ;
services< D
,D E
ILoggerF M
<M N
UserManagerN Y
<Y Z
ApplicationUserZ i
>i j
>j k
loggerl r
)r s
: 
base 
( 
store 
, 
optionsAccessor )
,) *
passwordHasher+ 9
,9 :
userValidators; I
,I J
passwordValidatorsK ]
,] ^
keyNormalizer_ l
,l m
errorsn t
,t u
servicesv ~
,~ 
logger
€ †
)
† ‡
{ 	
} 	
} 
} µ
lC:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Infrastructure\ExtendedClaimsProvider.cs
	namespace		 	
Mix		
 
.		 
Identity		 
.		 
Infrastructure		 %
{

 
public 

static 
class "
ExtendedClaimsProvider .
{ 
public 
static 
IEnumerable !
<! "
Claim" '
>' (
	GetClaims) 2
(2 3
ApplicationUser3 B
userC G
)G H
{ 	
List 
< 
Claim 
> 
claims 
=  
new! $
List% )
<) *
Claim* /
>/ 0
(0 1
)1 2
;2 3
foreach 
( 
var 
claim 
in !
user" &
.& '
Claims' -
)- .
{ 
claims 
. 
Add 
( 
CreateClaim &
(& '
claim' ,
., -
	ClaimType- 6
,6 7
claim8 =
.= >

ClaimValue> H
)H I
)I J
;J K
} 
return 
claims 
; 
} 	
public 
static 
Claim 
CreateClaim '
(' (
string( .
type/ 3
,3 4
string5 ;
value< A
)A B
{ 	
return 
new 
Claim 
( 
type !
,! "
value# (
,( )
ClaimValueTypes* 9
.9 :
String: @
)@ A
;A B
} 	
} 
} —
WC:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Interfaces\IUser.cs
	namespace 	
Mix
 
. 
Identity 
. 

Interfaces !
{		 
public

 

	interface

 
IUser

 
{ 
string 
Name 
{ 
get 
; 
} 
bool 
IsAuthenticated 
( 
) 
; 
IEnumerable   
<   
Claim   
>   
GetClaimsIdentity   ,
(  , -
)  - .
;  . /
}!! 
}"" ñ
vC:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Models\AccountViewModels\CreateRoleBindingModel.cs
	namespace 	
Mix
 
. 
Identity 
. 
Identity 
.  
Models  &
.& '
AccountViewModels' 8
{ 
public

 

class

 "
CreateRoleBindingModel

 '
{ 
[ 	
Required	 
] 
[ 	
StringLength	 
( 
$num 
, 
ErrorMessage '
=( )
$str* Y
,Y Z
MinimumLength[ h
=i j
$numk l
)l m
]m n
[ 	
Display	 
( 
Name 
= 
$str #
)# $
]$ %
[ 	
JsonProperty	 
( 
$str 
) 
] 
public 
string 
Name 
{ 
get  
;  !
set" %
;% &
}' (
} 
} µ
‚C:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Models\AccountViewModels\ExternalLoginConfirmationViewModel.cs
	namespace 	
Mix
 
. 
Identity 
. 
Models 
. 
AccountViewModels /
{ 
public		 

class		 .
"ExternalLoginConfirmationViewModel		 3
{

 
[ 	
Required	 
] 
[ 	
EmailAddress	 
] 
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
} 
} œ
vC:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Models\AccountViewModels\ExternalLoginViewModel.cs
	namespace 	
Mix
 
. 
Identity 
. 
Models 
. 
AccountViewModels /
{ 
public		 

class		 "
ExternalLoginViewModel		 '
{

 
[ 	
Required	 
] 
[ 	
EmailAddress	 
] 
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
} 
} ž
wC:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Models\AccountViewModels\ForgotPasswordViewModel.cs
	namespace 	
Mix
 
. 
Identity 
. 
Models 
. 
AccountViewModels /
{ 
public		 

class		 #
ForgotPasswordViewModel		 (
{

 
[ 	
Required	 
] 
[ 	
EmailAddress	 
] 
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
} 
} Ö
nC:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Models\AccountViewModels\LoginViewModel.cs
	namespace 	
Mix
 
. 
Identity 
. 
Models 
. 
AccountViewModels /
{ 
public		 

class		 
LoginViewModel		 
{

 
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
public 
string 
UserName 
{  
get! $
;$ %
set& )
;) *
}+ ,
[!! 	
Required!!	 
]!! 
["" 	
DataType""	 
("" 
DataType"" 
."" 
Password"" #
)""# $
]""$ %
public## 
string## 
Password## 
{##  
get##! $
;##$ %
set##& )
;##) *
}##+ ,
[++ 	
Display++	 
(++ 
Name++ 
=++ 
$str++ &
)++& '
]++' (
public,, 
bool,, 

RememberMe,, 
{,,  
get,,! $
;,,$ %
set,,& )
;,,) *
},,+ ,
public.. 
string.. 
	ReturnUrl.. 
{..  !
get.." %
;..% &
set..' *
;..* +
}.., -
}// 
}00 µ
uC:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Models\AccountViewModels\LoginWith2faViewModel.cs
	namespace 	
Mix
 
. 
Identity 
. 
Models 
. 
AccountViewModels /
{		 
public

 

class

 !
LoginWith2FaViewModel

 &
{ 
[ 	
Required	 
] 
[ 	
StringLength	 
( 
$num 
, 
ErrorMessage %
=& '
$str( f
,f g
MinimumLengthh u
=v w
$numx y
)y z
]z {
[ 	
DataType	 
( 
DataType 
. 
Text 
)  
]  !
[ 	
Display	 
( 
Name 
= 
$str ,
), -
]- .
public 
string 
TwoFactorCode #
{$ %
get& )
;) *
set+ .
;. /
}0 1
[ 	
Display	 
( 
Name 
= 
$str /
)/ 0
]0 1
public 
bool 
RememberMachine #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
bool 

RememberMe 
{  
get! $
;$ %
set& )
;) *
}+ ,
} 
} ˆ
~C:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Models\AccountViewModels\LoginWithRecoveryCodeViewModel.cs
	namespace 	
Mix
 
. 
Identity 
. 
Models 
. 
AccountViewModels /
{ 
public		 

class		 *
LoginWithRecoveryCodeViewModel		 /
{

 
[ 	
Required	 
] 
[ 	
DataType	 
( 
DataType 
. 
Text 
)  
]  !
[ 	
Display	 
( 
Name 
= 
$str '
)' (
]( )
public 
string 
RecoveryCode "
{# $
get% (
;( )
set* -
;- .
}/ 0
} 
} í
qC:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Models\AccountViewModels\RegisterViewModel.cs
	namespace		 	
Mix		
 
.		 
Identity		 
.		 
Models		 
.		 
AccountViewModels		 /
{

 
public 

class 
RegisterViewModel "
{ 
public 
string 
	FirstName 
{  !
get" %
;% &
set' *
;* +
}, -
public 
string 
LastName 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
NickName 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
List 
< 
SelectListItem "
>" #

UserClaims$ .
{/ 0
get1 4
;4 5
set6 9
;9 :
}; <
[ 	
Required	 
] 
public 
string 
UserName 
{  
get! $
;$ %
set& )
;) *
}+ ,
[ 	
Required	 
] 
[ 	
EmailAddress	 
] 
[ 	
Display	 
( 
Name 
= 
$str 
)  
]  !
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
[&& 	
Required&&	 
]&& 
['' 	
StringLength''	 
('' 
$num'' 
,'' 
ErrorMessage'' '
=''( )
$str''* h
,''h i
MinimumLength''j w
=''x y
$num''z {
)''{ |
]''| }
[(( 	
DataType((	 
((( 
DataType(( 
.(( 
Password(( #
)((# $
](($ %
[)) 	
Display))	 
()) 
Name)) 
=)) 
$str)) "
)))" #
]))# $
public** 
string** 
Password** 
{**  
get**! $
;**$ %
set**& )
;**) *
}**+ ,
[22 	
DataType22	 
(22 
DataType22 
.22 
Password22 #
)22# $
]22$ %
[33 	
Display33	 
(33 
Name33 
=33 
$str33 *
)33* +
]33+ ,
[44 	
Compare44	 
(44 
$str44 
,44 
ErrorMessage44 )
=44* +
$str44, b
)44b c
]44c d
public55 
string55 
ConfirmPassword55 %
{55& '
get55( +
;55+ ,
set55- 0
;550 1
}552 3
public77 
string77 
	ReturnUrl77 
{77  !
get77" %
;77% &
set77' *
;77* +
}77, -
}88 
}99 ï
vC:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Models\AccountViewModels\ResetPasswordViewModel.cs
	namespace 	
Mix
 
. 
Identity 
. 
Models 
. 
AccountViewModels /
{ 
public		 

class		 "
ResetPasswordViewModel		 '
{

 
[ 	
Required	 
] 
[ 	
EmailAddress	 
] 
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
[ 	
Required	 
] 
[ 	
StringLength	 
( 
$num 
, 
ErrorMessage '
=( )
$str* h
,h i
MinimumLengthj w
=x y
$numz {
){ |
]| }
[ 	
DataType	 
( 
DataType 
. 
Password #
)# $
]$ %
public 
string 
Password 
{  
get! $
;$ %
set& )
;) *
}+ ,
[&& 	
DataType&&	 
(&& 
DataType&& 
.&& 
Password&& #
)&&# $
]&&$ %
['' 	
Display''	 
('' 
Name'' 
='' 
$str'' *
)''* +
]''+ ,
[(( 	
Compare((	 
((( 
$str(( 
,(( 
ErrorMessage(( )
=((* +
$str((, b
)((b c
]((c d
public)) 
string)) 
ConfirmPassword)) %
{))& '
get))( +
;))+ ,
set))- 0
;))0 1
}))2 3
public11 
string11 
Code11 
{11 
get11  
;11  !
set11" %
;11% &
}11' (
}22 
}33 Æ
qC:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Models\AccountViewModels\SendCodeViewModel.cs
	namespace 	
Mix
 
. 
Identity 
. 
Models 
. 
AccountViewModels /
{		 
public

 

class

 
SendCodeViewModel

 "
{ 
public 
string 
SelectedProvider &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
ICollection 
< 
SelectListItem )
>) *
	Providers+ 4
{5 6
get7 :
;: ;
set< ?
;? @
}A B
public"" 
string"" 
	ReturnUrl"" 
{""  !
get""" %
;""% &
set""' *
;""* +
}"", -
public** 
bool** 

RememberMe** 
{**  
get**! $
;**$ %
set**& )
;**) *
}**+ ,
}++ 
},, “
mC:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Models\AccountViewModels\UserRoleModel.cs
	namespace 	
Mix
 
. 
Identity 
. 
Models 
. 
AccountViewModels /
{		 
public

 

class

 
UserRoleModel

 
{ 
[ 	
JsonProperty	 
( 
$str 
) 
]  
public 
string 
UserId 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	
JsonProperty	 
( 
$str 
) 
]  
public 
string 
RoleId 
{ 
get "
;" #
set$ '
;' (
}) *
[ 	
JsonProperty	 
( 
$str  
)  !
]! "
public 
string 
RoleName 
{  
get! $
;$ %
set& )
;) *
}+ ,
[ 	
JsonProperty	 
( 
$str $
)$ %
]% &
public 
bool 
IsUserInRole  
{! "
get# &
;& '
set( +
;+ ,
}- .
} 
} Ÿ

pC:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Models\AccountViewModels\UsersInRoleModel.cs
	namespace 	
Mix
 
. 
Identity 
. 
Identity 
.  
Models  &
.& '
AccountViewModels' 8
{ 
public 

class 
UsersInRoleModel !
{		 
[

 	
JsonProperty

	 
(

 
$str

 
)

 
]

 
public 
string 
Id 
{ 
get 
; 
set  #
;# $
}% &
[ 	
JsonProperty	 
( 
$str %
)% &
]& '
public 
List 
< 
string 
> 
EnrolledUsers )
{* +
get, /
;/ 0
set1 4
;4 5
}6 7
[ 	
JsonProperty	 
( 
$str $
)$ %
]% &
public 
List 
< 
string 
> 
RemovedUsers (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
} 
} Ÿ
mC:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Models\AccountViewModels\UserViewModel.cs
	namespace		 	
Mix		
 
.		 
Identity		 
.		 
Models		 
.		 
AccountViewModels		 /
{

 
public 

class 
UserViewModel 
{ 
public 
string 
Id 
{ 
get 
; 
set  #
;# $
}% &
public 
string 
UserName 
{  
get! $
;$ %
set& )
;) *
}+ ,
[ 	
DataType	 
( 
DataType 
. 
Password #
)# $
]$ %
public 
string 
Password 
{  
get! $
;$ %
set& )
;) *
}+ ,
[ 	
Display	 
( 
Name 
= 
$str *
)* +
]+ ,
[ 	
DataType	 
( 
DataType 
. 
Password #
)# $
]$ %
public 
string 
ConfirmPassword %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
string 
Name 
{ 
get  
;  !
set" %
;% &
}' (
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
[ 	
Display	 
( 
Name 
= 
$str %
)% &
]& '
public 
List 
< 
SelectListItem "
>" #

UserClaims$ .
{/ 0
get1 4
;4 5
set6 9
;9 :
}; <
} 
} 
sC:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Models\AccountViewModels\VerifyCodeViewModel.cs
	namespace 	
Mix
 
. 
Identity 
. 
Models 
. 
AccountViewModels /
{ 
public		 

class		 
VerifyCodeViewModel		 $
{

 
[ 	
Required	 
] 
public 
string 
Provider 
{  
get! $
;$ %
set& )
;) *
}+ ,
[ 	
Required	 
] 
public 
string 
Code 
{ 
get  
;  !
set" %
;% &
}' (
public## 
string## 
	ReturnUrl## 
{##  !
get##" %
;##% &
set##' *
;##* +
}##, -
[++ 	
Display++	 
(++ 
Name++ 
=++ 
$str++ 0
)++0 1
]++1 2
public,, 
bool,, 
RememberBrowser,, #
{,,$ %
get,,& )
;,,) *
set,,+ .
;,,. /
},,0 1
[44 	
Display44	 
(44 
Name44 
=44 
$str44 &
)44& '
]44' (
public55 
bool55 

RememberMe55 
{55  
get55! $
;55$ %
set55& )
;55) *
}55+ ,
}66 
}77 ¸
]C:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Models\ApplicationUser.cs
	namespace

 	
Mix


 
.

 
Identity

 
.

 
Models

 
{ 
public 

class 
ApplicationUser  
:! "
IdentityUser# /
{ 
[ 	
Required	 
] 
public 
DateTime 
JoinDate  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
bool 
	IsActived 
{ 
get  #
;# $
set% (
;( )
}* +
public 
System 
. 
DateTime 
LastModified +
{, -
get. 1
;1 2
set3 6
;6 7
}8 9
public 
string 

ModifiedBy  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
string 
RegisterType "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
string 
Avatar 
{ 
get "
;" #
set$ '
;' (
}) *
public 
string 
NickName 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
	FirstName 
{  !
get" %
;% &
set' *
;* +
}, -
public 
string 
LastName 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
Gender 
{ 
get "
;" #
set$ '
;' (
}) *
public 
int 
	CountryId 
{ 
get "
;" #
set$ '
;' (
}) *
public 
string 
Culture 
{ 
get  #
;# $
set% (
;( )
}* +
public 
DateTime 
? 
DOB 
{ 
get "
;" #
set$ '
;' (
}) *
public## 
virtual## 
ICollection## "
<##" #
IdentityUserRole### 3
<##3 4
string##4 :
>##: ;
>##; <
Roles##= B
{##C D
get##E H
;##H I
}##J K
=##L M
new##N Q
List##R V
<##V W
IdentityUserRole##W g
<##g h
string##h n
>##n o
>##o p
(##p q
)##q r
;##r s
public(( 
virtual(( 
ICollection(( "
<((" #
IdentityUserClaim((# 4
<((4 5
string((5 ;
>((; <
>((< =
Claims((> D
{((E F
get((G J
;((J K
}((L M
=((N O
new((P S
List((T X
<((X Y
IdentityUserClaim((Y j
<((j k
string((k q
>((q r
>((r s
(((s t
)((t u
;((u v
public-- 
virtual-- 
ICollection-- "
<--" #
IdentityUserLogin--# 4
<--4 5
string--5 ;
>--; <
>--< =
Logins--> D
{--E F
get--G J
;--J K
}--L M
=--N O
new--P S
List--T X
<--X Y
IdentityUserLogin--Y j
<--j k
string--k q
>--q r
>--r s
(--s t
)--t u
;--u v
}.. 
}// ð
XC:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Models\AspNetUser.cs
	namespace

 	
Mix


 
.

 
Identity

 
.

 
Models

 
{ 
public 

class 

AspNetUser 
: 
IUser #
{ 
private 
readonly  
IHttpContextAccessor -
	_accessor. 7
;7 8
public 

AspNetUser 
(  
IHttpContextAccessor .
accessor/ 7
)7 8
{ 	
	_accessor 
= 
accessor  
;  !
} 	
public"" 
string"" 
Name"" 
=>"" 
	_accessor"" '
.""' (
HttpContext""( 3
.""3 4
User""4 8
.""8 9
Identity""9 A
.""A B
Name""B F
;""F G
public** 
bool** 
IsAuthenticated** #
(**# $
)**$ %
{++ 	
return,, 
	_accessor,, 
.,, 
HttpContext,, (
.,,( )
User,,) -
.,,- .
Identity,,. 6
.,,6 7
IsAuthenticated,,7 F
;,,F G
}-- 	
public33 
IEnumerable33 
<33 
Claim33  
>33  !
GetClaimsIdentity33" 3
(333 4
)334 5
{44 	
return55 
	_accessor55 
.55 
HttpContext55 (
.55( )
User55) -
.55- .
Claims55. 4
;554 5
}66 	
}77 
}88 Ÿ
vC:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Models\ManageViewModels\AddPhoneNumberViewModel.cs
	namespace 	
Mix
 
. 
Identity 
. 
Models 
. 
ManageViewModels .
{ 
public		 

class		 #
AddPhoneNumberViewModel		 (
{

 
[ 	
Required	 
] 
[ 	
Phone	 
] 
[ 	
Display	 
( 
Name 
= 
$str &
)& '
]' (
public 
string 
PhoneNumber !
{" #
get$ '
;' (
set) ,
;, -
}. /
} 
} ½
vC:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Models\ManageViewModels\ChangePasswordViewModel.cs
	namespace 	
Mix
 
. 
Identity 
. 
Models 
. 
ManageViewModels .
{ 
public		 

class		 #
ChangePasswordViewModel		 (
{

 
[ 	
Required	 
] 
[ 	
DataType	 
( 
DataType 
. 
Password #
)# $
]$ %
[ 	
Display	 
( 
Name 
= 
$str *
)* +
]+ ,
public 
string 
OldPassword !
{" #
get$ '
;' (
set) ,
;, -
}. /
[ 	
Required	 
] 
[ 	
StringLength	 
( 
$num 
, 
ErrorMessage '
=( )
$str* h
,h i
MinimumLengthj w
=x y
$numz {
){ |
]| }
[ 	
DataType	 
( 
DataType 
. 
Password #
)# $
]$ %
[ 	
Display	 
( 
Name 
= 
$str &
)& '
]' (
public   
string   
NewPassword   !
{  " #
get  $ '
;  ' (
set  ) ,
;  , -
}  . /
[(( 	
DataType((	 
((( 
DataType(( 
.(( 
Password(( #
)((# $
](($ %
[)) 	
Display))	 
()) 
Name)) 
=)) 
$str)) .
))). /
]))/ 0
[** 	
Compare**	 
(** 
$str** 
,** 
ErrorMessage**  ,
=**- .
$str**/ i
)**i j
]**j k
public++ 
string++ 
ConfirmPassword++ %
{++& '
get++( +
;+++ ,
set++- 0
;++0 1
}++2 3
},, 
}-- Ÿ
zC:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Models\ManageViewModels\ConfigureTwoFactorViewModel.cs
	namespace 	
Mix
 
. 
Identity 
. 
Models 
. 
ManageViewModels .
{		 
public

 

class

 '
ConfigureTwoFactorViewModel

 ,
{ 
public 
string 
SelectedProvider &
{' (
get) ,
;, -
set. 1
;1 2
}3 4
public 
ICollection 
< 
SelectListItem )
>) *
	Providers+ 4
{5 6
get7 :
;: ;
set< ?
;? @
}A B
} 
} ¢
nC:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Models\ManageViewModels\FactorViewModel.cs
	namespace 	
Mix
 
. 
Identity 
. 
Models 
. 
ManageViewModels .
{ 
public 

class 
FactorViewModel  
{ 
public 
string 
Purpose 
{ 
get  #
;# $
set% (
;( )
}* +
} 
} Ñ	
mC:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Models\ManageViewModels\IndexViewModel.cs
	namespace 	
Mix
 
. 
Identity 
. 
Models 
. 
ManageViewModels .
{		 
public

 

class

 
IndexViewModel

 
{ 
public 
bool 
HasPassword 
{  !
get" %
;% &
set' *
;* +
}, -
public 
IList 
< 
UserLoginInfo "
>" #
Logins$ *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
public"" 
string"" 
PhoneNumber"" !
{""" #
get""$ '
;""' (
set"") ,
;"", -
}"". /
public** 
bool** 
	TwoFactor** 
{** 
get**  #
;**# $
set**% (
;**( )
}*** +
public22 
bool22 
BrowserRemembered22 %
{22& '
get22( +
;22+ ,
set22- 0
;220 1
}222 3
}33 
}44 Ê
tC:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Models\ManageViewModels\ManageLoginsViewModel.cs
	namespace		 	
Mix		
 
.		 
Identity		 
.		 
Models		 
.		 
ManageViewModels		 .
{

 
public 

class !
ManageLoginsViewModel &
{ 
public 
IList 
< 
UserLoginInfo "
>" #
CurrentLogins$ 1
{2 3
get4 7
;7 8
set9 <
;< =
}> ?
public 
IList 
<  
AuthenticationScheme )
>) *
OtherLogins+ 6
{7 8
get9 <
;< =
set> A
;A B
}C D
} 
} Ñ
sC:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Models\ManageViewModels\RemoveLoginViewModel.cs
	namespace 	
Mix
 
. 
Identity 
. 
Models 
. 
ManageViewModels .
{ 
public 

class  
RemoveLoginViewModel %
{ 
public 
string 
LoginProvider #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public 
string 
ProviderKey !
{" #
get$ '
;' (
set) ,
;, -
}. /
} 
} Ó
sC:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Models\ManageViewModels\SetPasswordViewModel.cs
	namespace 	
Mix
 
. 
Identity 
. 
Models 
. 
ManageViewModels .
{ 
public		 

class		  
SetPasswordViewModel		 %
{

 
[ 	
Required	 
] 
[ 	
StringLength	 
( 
$num 
, 
ErrorMessage '
=( )
$str* h
,h i
MinimumLengthj w
=x y
$numz {
){ |
]| }
[ 	
DataType	 
( 
DataType 
. 
Password #
)# $
]$ %
[ 	
Display	 
( 
Name 
= 
$str &
)& '
]' (
public 
string 
NewPassword !
{" #
get$ '
;' (
set) ,
;, -
}. /
[ 	
DataType	 
( 
DataType 
. 
Password #
)# $
]$ %
[ 	
Display	 
( 
Name 
= 
$str .
). /
]/ 0
[ 	
Compare	 
( 
$str 
, 
ErrorMessage  ,
=- .
$str/ i
)i j
]j k
public   
string   
ConfirmPassword   %
{  & '
get  ( +
;  + ,
set  - 0
;  0 1
}  2 3
}!! 
}"" ñ
yC:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Models\ManageViewModels\VerifyPhoneNumberViewModel.cs
	namespace 	
Mix
 
. 
Identity 
. 
Models 
. 
ManageViewModels .
{ 
public		 

class		 &
VerifyPhoneNumberViewModel		 +
{

 
[ 	
Required	 
] 
public 
string 
Code 
{ 
get  
;  !
set" %
;% &
}' (
[ 	
Required	 
] 
[ 	
Phone	 
] 
[ 	
Display	 
( 
Name 
= 
$str &
)& '
]' (
public 
string 
PhoneNumber !
{" #
get$ '
;' (
set) ,
;, -
}. /
} 
} ›
bC:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Repositories\AuthRepository.cs
	namespace 	
Mix
 
. 
Identity 
. 
Repositories #
{ 
public 

class 
AuthRepository 
:  !
UserManager" -
<- .
ApplicationUser. =
>= >
{ 
public 
AuthRepository 
( 

IUserStore (
<( )
ApplicationUser) 8
>8 9
store: ?
,? @
IOptionsA I
<I J
IdentityOptionsJ Y
>Y Z
optionsAccessor[ j
,j k
IPasswordHasher 
< 
ApplicationUser +
>+ ,
passwordHasher- ;
,; <
IEnumerable= H
<H I
IUserValidatorI W
<W X
ApplicationUserX g
>g h
>h i
userValidatorsj x
,x y
IEnumerable 
< 
IPasswordValidator *
<* +
ApplicationUser+ :
>: ;
>; <
passwordValidators= O
,O P
ILookupNormalizerQ b
keyNormalizerc p
,p q"
IdentityErrorDescriber "
errors# )
,) *
IServiceProvider+ ;
services< D
,D E
ILoggerF M
<M N
UserManagerN Y
<Y Z
ApplicationUserZ i
>i j
>j k
loggerl r
)r s
: 
base 
( 
store 
, 
optionsAccessor )
,) *
passwordHasher+ 9
,9 :
userValidators; I
,I J
passwordValidatorsK ]
,] ^
keyNormalizer_ l
,l m
errorsn t
,t u
servicesv ~
,~ 
logger
€ †
)
† ‡
{ 	
} 	
} 
} æ
[C:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Services\EmailSender.cs
	namespace 	
Mix
 
. 
Identity 
. 
Services 
{		 
public

 

class

 
EmailSender

 
:

 
IEmailSender

 +
{ 
public 
Task 
SendEmailAsync "
(" #
string# )
email* /
,/ 0
string1 7
subject8 ?
,? @
stringA G
htmlMessageH S
)S T
{ 	
return 
Task 
. 
CompletedTask %
;% &
} 	
} 
} €
ZC:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Services\ISmsSender.cs
	namespace 	
Mix
 
. 
Identity 
. 
Services 
{ 
public		 

	interface		 

ISmsSender		 
{

 
Task 
SendSmsAsync 
( 
string  
number! '
,' (
string) /
message0 7
)7 8
;8 9
} 
} ˜

_C:\_workspace\github\mixcore\mix.identity\src\Mix.Identity\Identity\Services\MessageServices.cs
	namespace 	
Mix
 
. 
Identity 
. 
Services 
{		 
public

 

class

 "
AuthEmailMessageSender

 '
:

( )
IEmailSender

* 6
{ 
public 
Task 
SendEmailAsync "
(" #
string# )
email* /
,/ 0
string1 7
subject8 ?
,? @
stringA G
htmlMessageH S
)S T
{ 	
return 
Task 
. 

FromResult "
(" #
$num# $
)$ %
;% &
} 	
} 
public 

class  
AuthSmsMessageSender %
:& '

ISmsSender( 2
{ 
public"" 
Task"" 
SendSmsAsync""  
(""  !
string""! '
number""( .
,"". /
string""0 6
message""7 >
)""> ?
{## 	
return%% 
Task%% 
.%% 

FromResult%% "
(%%" #
$num%%# $
)%%$ %
;%%% &
}&& 	
}'' 
}(( 