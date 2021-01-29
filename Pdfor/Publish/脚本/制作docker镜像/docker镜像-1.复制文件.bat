echo '复制文件'

::制作镜像
xcopy "..\..\04.服务站点\Pdfor"  "..\..\06.Docker\制作镜像\pdfor\app\pdfor"  /e /i /r /y


:: 部署文件
xcopy  "..\..\04.服务站点\Pdfor\appsettings.json" "..\..\06.Docker\部署文件" 

