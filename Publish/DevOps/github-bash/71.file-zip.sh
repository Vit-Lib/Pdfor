set -e


#---------------------------------------------------------------------
#(x.1)参数
args_="

export basePath=/root/temp/svn

export version=`grep '<Version>' $(grep '<pack/>\|<publish>' ${basePath} -r --include *.csproj -l | head -n 1) | grep -oP '>(.*)<' | tr -d '<>'`

export name=pdfor

# "

 



#----------------------------------------------
echo "压缩文件"

docker run --rm -i \
-v $basePath:/root/code \
serset/filezip bash -c "
set -e

releasePath=/root/code/Publish/release

for dirname in \`ls /root/code/Publish/release/release\`
do
  if [ -d \$releasePath/release/\$dirname ]
  then
    filezip zip -p -i \$releasePath/release/\$dirname -o \$releasePath/release-zip/${name}-\${dirname}-${version}.zip 
  fi
done

echo zip files:
ls /root/code/Publish/release/release-zip

"