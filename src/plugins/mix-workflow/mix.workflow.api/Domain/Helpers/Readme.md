# Notes :
- Must set db context from injected service to ViewModel (MixHeart cannot init db context parameterless
```
TView vm = new();
vm.SetDbContext(injectedContext);
```
