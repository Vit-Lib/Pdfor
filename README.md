# Pdfor

代码地址：<https://github.com/serset/Pdfor>

Pdfor为一款跨平台的pdf转换器。  
可以把doc、docx、xls、xlsx、ppt、pptx 、html、txt 等文件转换为pdf格式。  
通过http api方式对外提供服务,接口地址为 /Pdfor/ConvertToPdf。  
运行环境为net core 2.1。  


linux平台支持LibReOffice驱动。  
windows平台支持使用Office 和 LibReOffice驱动。



# docker部署
pdfor可以通过docker部署，docker镜像地址为 serset/pdfor

``` bash

# 前台启动 
docker run -it --rm -p 4301:4301 serset/pdfor


# 后台启动 
docker run --name=pdfor --restart=always -d -p 4301:4301 serset/pdfor

# 启动后访问 http://ip:4301

```



