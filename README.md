Repository for code from my 2018-Feb-01 blog post:

### [Reusable Dependency Injection for Azure Function Apps](https://mcguirev10.com/2018/02/01/reusable-dependency-injection-azure-function.html)

---

Based upon work in these repos:

[BorisWilhelms](https://github.com/BorisWilhelms/azure-function-dependency-injection)

[yuka1984](https://github.com/yuka1984/azure-function-dependency-injection)

---

2018-04 About Functions V2:

I'd hoped to update this library to support Functions V2, however all of the `netstandard20`-compatible releases of the SDK and the webjob assemblies throw various binding errors (most recently while running locally, and in all cases when deployed to Azure). I spent a couple days researching the problem and trying various things, and I've concluded that Functions are moving in a direction that may make custom trigger bindings impossible to build.

Unfortunately [this](https://github.com/Azure/azure-webjobs-sdk/wiki/Creating-custom-input-and-output-bindings) Functions wiki even states "Custom triggers are not available for Azure Functions," and various Microsoft folks have responded to Stack Overflow questions that custom triggers aren't in-scope.

Since the service registration custom trigger is part of what made this DI library so convenient to use, and given that Microsoft is working on adding DI support to Functions (it sounds like we might have to wait for the runtime 3.0 release -- still Functions V2, terrible choice of names there), I'm not burning any more time (or Azure billing) on V2 support.

The V2 changes likely would have been pretty simple, apart from the new extensions installation/registration gynmastics (which supposedly will go away). I'll leave the changes in a separate branch in case someone else has more time to kill than I do!

https://github.com/MV10/Azure.Functions.Dependency.Injection/tree/FunctionsV2

See also:

### [Service Locator for Azure Functions V2](https://mcguirev10.com/2018/04/03/service-locator-azure-functions-v2.html)
