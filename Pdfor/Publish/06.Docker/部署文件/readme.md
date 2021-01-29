# docker部署pdfor

Pdfor为一款跨平台的pdf转换器。

可以把doc、docx、xls、xlsx、ppt、pptx 、html、txt 等文件转换为pdf格式。

通过http api方式对外提供服务,接口地址为 /Pdfor/ConvertToPdf。

运行环境为net core 2.1。


# 命令行启动容器

``` bash

# 前台启动 
docker run -it --rm -p 4301:4301 sersms/pdfor


# 后台启动 
docker run --name=pdfor --restart=always -p 4301:4301 sersms/pdfor

# 启动后访问 http://ip:4301

```


 