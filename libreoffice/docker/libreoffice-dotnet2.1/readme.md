# docker部署libreoffice:dotnet2.1


# 命令行启动容器
> office文件存放在宿主机的/root/docker/libreoffice中并映射到容器，可按需修改。
> libreoffice亦可改为soffice

``` bash
 
docker run -it --rm -v /root/docker/libreoffice:/root/libreoffice sersms/libreoffice:dotnet2.1 /bin/sh


# libreoffice命令demo

# 查看帮助
libreoffice --help

# 转换doc为pdf文件（转换后的文件为 /root/libreoffice/doc.pdf）
libreoffice --headless  --convert-to pdf --outdir "/root/libreoffice" "/root/libreoffice/doc.docx"

# 转换xlsx为pdf文件
libreoffice --headless  --convert-to pdf --outdir "/root/libreoffice" "/root/libreoffice/xls.xlsx"

# 转换pptx为pdf文件
libreoffice --headless  --convert-to pdf --outdir "/root/libreoffice" "/root/libreoffice/ppt.pptx"

```



# 直接转换为pdf
> office文件存放在宿主机的/root/docker/libreoffice中并映射到容器，可按需修改。
> libreoffice亦可改为soffice

``` bash
 
docker run -it --rm \
-v /root/docker/libreoffice:/root/libreoffice \
sersms/libreoffice:dotnet2.1 \
libreoffice "--headless" "--convert-to" pdf "--outdir" "/root/libreoffice" "/root/libreoffice/xls.xlsx"


```