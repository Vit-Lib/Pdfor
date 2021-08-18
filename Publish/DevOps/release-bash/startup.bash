set -e

# cd /root/temp/svn/Publish/DevOps/release-bash;bash startup.bash;

#----------------------------------------------
#(x.1)当前路径 
curPath=$PWD

cd $curPath/../../..
export basePath=$PWD
cd $curPath





#----------------------------------------------
echo "(x.2)get version" 
export version=`grep '<Version>' $(grep '<pack>\|<publish>' ${basePath} -r --include *.csproj -l | head -n 1) | grep -oP '>(.*)<' | tr -d '<>'`
echo $version

 


#----------------------------------------------
echo "(x.3)自动发布 $name-$version"

for file in *.sh
do
    echo %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
    echo "[$(date "+%H:%M:%S")]" bash $file
    bash $file
done






 
#----------------------------------------------
#(x.9)
cd $curPath
