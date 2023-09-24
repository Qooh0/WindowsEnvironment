# WindowsEnvironment

~~set Environment Variable on windows.~~  
Use SET command.

## Objective.

I want to set environment variables easily.

## License

Apache License 2.0

## Requirements.

- Windows
- .net 8

## How To Use

### set environment variable

### get environment variable

#### only cmd

```C#
windowsEnvironment.exe path
```

#### A plane file with a list of keys

test.txt

```txt
path
simpleEnv
```

```cmd
windowsEnvironment.exe export -f test.txt
```

#### A json file with a list of keys

```json
{"path":"","qtest":""}
```

### How To Compile

1. compile

```C#
dotnet build
```

Production

```C#
dotnet publish -p:PublishSingleFile=true -p:AssemblyName=windowsEnvironment
```

2. use

```C#
windowsEnvironment.exe export
```

## メモ

SubCommand になるのは、SubCommands の関数名。
SubCommands ディレクトリは、処理を書くだけ。

読み書きファイルは UTF-8 限定
