1，源码在src目录
2，默认提供了vs2013工程编译，进入vs2013目录，打开protobuf.sln，build时选择ALL_BUILD或者protoc都可以，build的结果protoc.exe默认会放在proto\tool下，便于直接测试.proto生成
3，如果希望用其它方式编译，请自行从cmake搞