set -e


#---------------------------------------------------------------------
#(x.1)参数
args_="

export basePath=/root/temp/svn

# "

 
#---------------------------------------------------------------------
#(x.2)
publishPath="$basePath/Publish/release/release/Station(net5.0)"
dockerPath=$basePath/Publish/release/release/docker-deploy



#----------------------------------------------
echo "(x.3)copy dir"
\cp -rf "$basePath/Publish/ReleaseFile/docker-deploy/." "$dockerPath"

\cp -f "$basePath/README.md" "$dockerPath/pdfor"
\cp -f "$basePath/Pdfor部署说明.txt" "$dockerPath/pdfor"

\cp -f "$publishPath/pdfor/appsettings.json" "$dockerPath/pdfor"

