在线文件阅览系统，支持doc、xls、ppt、pdf等格式文件
运行环境：在IE中运行，系统中装有office2013

1.flexpaper，swftools 下载地址：https://flowpaper.com/download/    网站开发的free的那一列就好，里面有flexpaper的java，php，c#版本的DEMO
2.实现机理：ppt、xls、doc通过引入office的com组件实现转成pdf，然后通过swftools模拟命令行转化为swf，最后通过flexpaper经由flash显示在网页上。
3.com组件Microsoft Word 12.0 Object Library  、Microsoft PowerPoint 12.0 Object Library 、Microsoft Excel 12.0 Object Library、Microsoft Office Object Library
网上参考大部分是2007版的office要洗在一个插件“SaveAsPDFandXPS.exe”，地址是"https://www.microsoft.com/en-us/download/details.aspx?id=7"  我用的是office2016版本，
不用下载插件，但是有的类不能用，将代码中ApplicationClass改成了Application才能运行成功
4.参考博客：（1）http://www.cnblogs.com/shenchao/p/3950942.html
			（2）http://www.cnblogs.com/amylis_chen/p/3754814.html
			（3）http://www.cnblogs.com/yunfeifei/p/4520414.html
5.由于swftools转化是用的模拟命令行，所以所有转化的参数都不能带空格，这就要求工程路径，文件名不能带空格
6.flexpaper.js中为使得目录对应将61行    “src						    : _jsDirectory+"../FlexPaperViewer.swf",”     
		转化为   “src						    : _jsDirectory+"FlexPaperViewer.swf",”
7.PDF2JSON  把pdf转化成文字与位置或状态的json文件，  PDFTk  强大的pdf处理工具，分割，合并，旋转，编码，解码
8.手势识别方式为握拳