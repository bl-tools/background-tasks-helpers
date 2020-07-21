## Installation

Add package to endpoint project ('{solutionName}.App')
```
Install-Package Common.QuartzNetBackgroundTaskHelpers
```

## Registration

In DependencyConfig register using helper below.

```c#
services.AddBackgroundTasks(context.Configuration, "BackgroundTasks");
```

Tasks are defined via configuration. Configuratino section name passed as parameter.

Sample config section
```json
{
  "BackgroundTasks": {
    "TaskName1": {
      "TaskType": "Abc.Product.Service.Tasks.ImportTask,Abc.Product.Service",
      "IntervalSeconds": 20,
      "StartDelaySeconds": 2
    },
    "TaskName2": {
      "TaskType": "Abc.Product.Service.Tasks.ExportTask,Abc.Product.Service",
      "IntervalSeconds": 20,
      "StartDelaySeconds": 5
    }
  }
}
```
Note: TaskName1 and TaskName2 are just descriptive names of tasks. 
TaskType - fully qualified type name of task class (the one that implements `IJob`)

## Define Tasks
Tasks are defined in project '{solutionName}.Service'.
Need to install Quartz.NET package:
```
Install-Package Quartz
```

Then create task classes. The only requirement - implement interface `IJob`.
Also `DisallowConcurrentExecution` attribute can be used to prevent multiple copies of the same task to run simultaneously.

Dependency Injections is fully supported. 