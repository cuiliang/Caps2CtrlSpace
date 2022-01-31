# Caps2CtrlSpace
CapsLock 切换输入法中英文
# 一种新的方法：通过Microsoft PowerToys软件修改按键映射
*使用该方法不需要添加开机启动项，同时更为稳定*

Microsoft PowerToys 中现在已经提供了修改按键映射的工具：键盘管理器->密钥->重映按键

点击重映按键在新页面中点击加号按钮添加新的映射，按下或在下拉栏菜单中选择对应的按键，建立我们需要的CapsLock 到Ctrl+ 空格的映射，确认无误后点击右上角确定键保存。
[![HiKiXq.png](https://s4.ax1x.com/2022/01/31/HiKiXq.png)](https://imgtu.com/i/HiKiXq)
软件会提示我们该操作会导致原始功能不可用，点击继续
[![HiKABV.png](https://s4.ax1x.com/2022/01/31/HiKABV.png)](https://imgtu.com/i/HiKABV)
完成后即可实现CapsLock 到Ctrl+ 空格的按键映射
[![HiKK39.png](https://s4.ax1x.com/2022/01/31/HiKK39.png)](https://imgtu.com/i/HiKK39)
获取Microsoft PowerToys :

环境要求：
- Windows 11 or Windows 10 v1903 (18362) or newer.
- NET Core 3.1.22 Desktop Runtime or a newer 3.1.x runtime. The installer will handle this if not present.

软件发行地址：

[Microsoft 应用商店](https://aka.ms/getPowertoys)

[Github项目发行页面](https://github.com/microsoft/PowerToys/releases)


PS：MSDN 关于此功能的描述文档：
[Keyboard Manager 检测
](https://docs.microsoft.com/zh-cn/windows/powertoys/keyboard-manager#keys-that-cannot-be-remapped)

# Caps2CtrlSpace原理
监听按键，如果是Capslock，转换成Ctrl+Space


# 开发环境
 VS2019
 
#  Exe下载
https://github.com/cuiliang/Caps2CtrlSpace/releases

  
#  使用
如需提权（在以管理员身份运行的程序中生效），请将其复制到`c:\program files`目录中使用。
启动后，会自动隐藏窗口显示在系统托盘内一个桔黄色的小点的图标。 这时候可以按capslock来切换中英文了。
 
*自动启动*
 
双击系统托盘图标，在打开的窗口中选中自动启动的选项即可。
 
