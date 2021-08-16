set -e


#---------------------------------------------------------------------
#(x.1)参数
args_="

export basePath=/root/temp/svn

export version=`grep '<Version>' $(grep '<pack/>\|<publish>' ${basePath} -r --include *.csproj -l | head -n 1) | grep -oP '>(.*)<' | tr -d '<>'`

export DOCKER_USERNAME=serset
export DOCKER_PASSWORD=xxx

# "






#---------------------------------------------------------------------
#(x.3)docker-构建镜像并推送到 Docker Hub

docker login -u $DOCKER_USERNAME -p $DOCKER_PASSWORD


dockerPath=$basePath/Publish/release/release/docker-image

for dockerName in `ls $dockerPath`
do
  if [ -d $dockerPath/$dockerName ]
  then 
    echo "docker build $dockerName"
    docker build -t $DOCKER_USERNAME/$dockerName:$version -t $DOCKER_USERNAME/$dockerName $dockerPath/$dockerName 

    docker push $DOCKER_USERNAME/$dockerName:$version
    docker push $DOCKER_USERNAME/$dockerName

    docker rmi $DOCKER_USERNAME/$dockerName:$version
    docker rmi $DOCKER_USERNAME/$dockerName
  fi
done



 
