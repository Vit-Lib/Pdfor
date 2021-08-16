set -e


#---------------------------------------------------------------------
#(x.1)参数
args_="

export basePath=/root/temp/svn

# "


#----------------------------------------------
echo "(x.2)netVersion"
netVersion=net5.0



#---------------------------------------------- 
echo "(x.3)publish $netVersion"

#修改csproj文件中的版本号
cd $basePath
sed -i 's/netcoreapp2.1/'"$netVersion"'/g'  `grep -a '<publish>' . -rl --include *.csproj`

cd $basePath/Publish/DevOps/release-bash
bash 40.Station-publish.sh;


#还原csproj文件中的版本号为netcoreapp2.1
cd $basePath
sed -i 's/'"$netVersion"'/netcoreapp2.1/g'  `grep -a '<publish>' . -rl --include *.csproj`


cd $basePath/Publish/DevOps/release-bash

