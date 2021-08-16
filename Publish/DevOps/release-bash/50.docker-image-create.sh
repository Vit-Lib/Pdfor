set -e


#---------------------------------------------------------------------
#(x.1)参数
args_="

export basePath=/root/temp/svn

# "

 
#---------------------------------------------------------------------
#(x.2)
publishPath="$basePath/Publish/release/release/Station(net5.0)"
dockerPath=$basePath/Publish/release/release/docker-image



#---------------------------------------------------------------------
echo "(x.3)copy dir"
\cp -rf "$basePath/Publish/ReleaseFile/docker-image/." "$dockerPath"


#---------------------------------------------------------------------
echo "(x.4)copy station"
#查找所有需要发布的项目并copy
cd $basePath
for file in $(grep -a '<docker>' . -rl --include *.csproj)
do
	cd $basePath
	
	#get publishName
	publishName=`grep '<publish>' $file -r | grep -oP '>(.*)<' | tr -d '<>'`
	
	#get dockerName
	dockerName=`grep '<docker>' $file -r | grep -oP '>(.*)<' | tr -d '<>'`

	echo copy $dockerName
	\cp -rf "$publishPath/$publishName/." "$dockerPath/$dockerName/app"
done


