# xiaoyi-sharp 小易 AI 机器人（智能体）
<p align="center">
  <a href="https://github.com/zhulige/xiaoyi-sharp/releases/latest">
    <img src="https://img.shields.io/github/v/release/zhulige/xiaoyi-sharp?style=flat-square&logo=github&color=blue" alt="Release"/>
  </a>
  <a href="https://opensource.org/licenses/MIT">
    <img src="https://img.shields.io/badge/License-MIT-green.svg?style=flat-square" alt="License: MIT"/>
  </a>
  <a href="https://github.com/zhulige/xiaoyi-sharp/stargazers">
    <img src="https://img.shields.io/github/stars/zhulige/xiaoyi-sharp?style=flat-square&logo=github" alt="Stars"/>
  </a>
  <a href="https://github.com/zhulige/xiaoyi-sharp/releases/latest">
    <img src="https://img.shields.io/github/downloads/zhulige/xiaoyi-sharp/total?style=flat-square&logo=github&color=52c41a1&maxAge=86400" alt="Download"/>
  </a>
</p>

<p align="center">
  简体中文 | English
</p>

## 项目简介 
小易 AI 机器人（智能体）是一个使用 C# 语言开发的智能体客户端，功能包括语音对话聊天、多模态、物联控制、机器视觉、ROS等。
当前项目兼容小智ESP32。

**跨平台支持**：本项目支持以下平台：
- **操作系统**：Windows、MacOS、Linux、Android
- **硬件平台**：x86、x86_64、arm、arm_64
- **开发板**：ASUS Tinker Board2s、Raspberry Pi

**支持服务端**:本项目支持一下服务端：
- **小智ESP32 ❤ 扣子Coze X 豆包**：http://xiaozhi.nbee.net
- **小智**：https://xiaozhi.me

## 运行指南

要运行本项目，你需要确保你的系统已经安装了 .NET Core SDK（推荐安装.net 8.0 、.net 9.0）。如果尚未安装，可以从 [官方网站](https://dotnet.microsoft.com/zh-cn/) 下载并安装适合你系统的版本。安装成功后，你可以按照以下步骤运行项目：
```bash
cd 到指定目录
dotnet run
```

## 项目组成

### 基础库

你可以使用它很快的创建一个自己的小智客户端应用。

``` C#
using XiaoYiSharp;

XiaoYiAgent _xiaoYiAgent = new XiaoYiAgent();
_xiaoyiAgent.OnMessageEvent += XiaoyiAgent_OnMessageEvent;
_xiaoyiAgent.OnAudioEvent += XiaoyiAgent_OnAudioEvent;
_xiaoyiAgent.OnIotThingsEvent += XiaoyiAgent_OnIotThingsEvent;
_xiaoyiAgent.IotThings = "";
_xiaoyiAgent.Start();
```

## 计划项目
<img src="doc/ErgoJr_assembly.gif" width="480" />

## 相关资源
https://opus-codec.org/downloads/
