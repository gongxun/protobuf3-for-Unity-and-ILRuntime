基于google官方3.4.1版本的protobuf c#-runtime以及3.4.0版本的protoc修改而来，可用于ILRuntime，同时移除了不常用的功能，精简了生成代码的体积.
当用于ILRuntime时，protobuf-runtime放在主工程（不可热更），让解析pb的代码跑在CLR上，提供更快的解析速度; 从.proto生成的c#代码放在热更dll工程(可热更).

已知限制：
  1，只支持proto3协议，不支持proto2
  2, 不支持WellKnownTypes，也就是.proto里不支持Any、TimeSpan、Duration等google.protobuf下预定义的.proto类型
  3，不支持JsonParser、FileDescriptor等反射功能
  4，.proto里不支持Map和oneof - 实现上应该是用到了反射，没细看，不确定能否在不支持反射的情况支持这个.
  5，protoc.exe不支持中文路径,也就是.proto不要放在中文目录下

修改的内容：
  注：runtime部分本质上可以改动很少（移除IMessage的各种继承和不会用到的接口方法，处理下编译错误就行），但出于个人习惯，把不会（被精简过的protoc生成的代码）用到的runtime部分全部都砍掉了，
      比如Relfection目录、WellKnown目录、JsonFromatter目录等都整个砍掉了. 这部分不砍掉也是没任何问题的，如果不砍掉，后续希望把protoc砍掉的部分功能加回来也更方便。

  1, runtime
     兼容.net framework 3.5
  
  2, runtime & protroc 
     移除了反射部分
  
  3，runtime 
     移除了WellKnownTypes
     注: WellKnownTypes只是从google默认定义的一组(常用).proto文件生成的代码，随runtime自带，因为我们改了runtime,所以自带的这些代码（从老的protoc生成）会有编译错误，如果你需要这个功能，处理一下编译错误就好，我是嫌麻烦整个都砍了.

  4, runtime & protoc
     移除了Clone等(我们项目)用不到的方法
  
  5, runtime & protoc
     移除了各种(我们项目)不需要的继承和接口，以精简ILRuntime的跨域继承.
  
  6, runtime & protoc
     移除了ToString()方法，因为默认实现需要用到FileDescriptor; 提供了Dumper.DumpAsString()方法来替代ToString方法，打印一个pb obj中所有filed的name和value，便于调试.
     注：Dumper只有配合精简过的protoc生成代码才能正常使用，因为里面有个假设: pb obj中所有的public NonStatic getter都是.proto中定义的field，而原版protoc生成的代码并不满足这个假设.


目录结构：
1，old - 未修改的原版pb:
   1.1，Google.Protobuf
       protobuf的runtime

   1.2  Protoc_3.4.0_bin
       预生成好的windows版.proto编译器，从.proto生成c#文件

   1.3 protoc_3.4.0_src
       protoc源码，已生成vs2013工程

2, new - 修改后的可用于ILRuntime的pb:
  2.1，Google.Protobuf
       protobuf的runtime，放在主工程
  
  2.2, Protoc_3.4.0_bin
      预生成好的windows版.proto编译器，从.proto生成c#文件，这些生成文件放在热更工程

  2.3 protoc_3.4.0_src
     修改后的protoc源码，已生成vs2013工程
  
  *2.4, Adapt_IMessage.cs和AdaptHelper.cs
      ILRuntime跨域继承适配器，只需要适配IMessage接口就行，.proto生成代码都是继承于这个接口.放到主工程，ILRutime初始化时注册这个适配继承器。
      如果不用ILRuntime，不需要此文件.

3, test - 测试用的.proto和test case




