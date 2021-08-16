set -e


#---------------------------------------------------------------------
#(x.1)参数
args_="

export basePath=/root/temp/svn

# "


#----------------------------------------------
echo "(x.2)获取netVersion" 
netVersion=`grep '<TargetFramework>' $(grep '<publish>' ${basePath} -r --include *.csproj -l | head -n 1) | grep -oP '>(.*)<' | tr -d '<>'`


publishPath=$basePath/Publish/release/release/Station\($netVersion\)
echo publish Station
echo dotnet version: $netVersion


#----------------------------------------------
echo "(x.3)查找所有需要发布的项目并发布"


mkdir -p $publishPath

docker run -i --rm \
--env LANG=C.UTF-8 \
-v $basePath/Publish/release/.nuget:/root/.nuget \
-v $basePath:/root/code \
serset/dotnet:sdk-6.0 \
bash -c "
set -e

basePath=/root/code
publishPath=\$basePath/Publish/release/release/Station\($netVersion\)

#(x.3)查找所有需要发布的项目并发布
cd \$basePath
for file in \$(grep -a '<publish>' . -rl --include *.csproj)
do
	cd \$basePath
	
	#get publishName
	publishName=\`grep '<publish>' \$file -r | grep -oP '>(.*)<' | tr -d '<>'\`

	echo publish \$publishName

	#publish
	cd \$(dirname \"\$file\")
	dotnet build --configuration Release
	dotnet publish --configuration Release --output \"\$publishPath/\$publishName\"

	#copy xml
	for filePath in bin/Release/$netVersion/*.xml ; do \\cp -rf \$filePath \"\$publishPath/\$publishName\";done
done


#(x.4)copy dir
\cp -rf \$basePath/Publish/ReleaseFile/Station/. \"\$publishPath\"


"



echo 'publish succeed！'





