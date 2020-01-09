# Conventions

## Asynchronous programming
- По умолчанию в API реализованы только асинхронные методы.
- Любой асинхронный метод должен иметь суффикс Async в конце, даже если нет похожего синхронного метода.  
Пример: `Task<IValidationResult> ValidateAsync(T instance, CancellationToken cancellationToken)`.

## Comments
- При написании комментариев необходимо ставить пробел после последнего слеша.
Пример: `// TODO: Fix this shit.`

## Classes 
- Именовать файлы partial-классов необходимо с указанием указанием их источника и назначения.
Пример: `EFRepository.readonly.cs` и `EFRepository.writeonly.cs`

## Members
- Public, Internal и Protected свойства и поля именуются с помощью Pascal case. В то время, как Private свойства и поля именуются с помощью Camel case и префикса "_".