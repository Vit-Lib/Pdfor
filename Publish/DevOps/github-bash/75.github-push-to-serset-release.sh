set -e


#---------------------------------------------------------------------
#(x.1)参数
args_="

export basePath=/root/temp/svn

export version=`grep '<Version>' $(grep '<pack>\|<publish>' ${basePath} -r --include *.csproj -l | head -n 1) | grep -oP '>(.*)<' | tr -d '<>'`

export name=pdfor

export export GIT_SSH_SECRET=xxxxxx

# "





#----------------------------------------------
echo "github-提交release文件到serset/release仓库"
# releaseFile=$basePath/Publish/release/release-zip

#复制ssh key
echo "${GIT_SSH_SECRET}" > $basePath/Publish/release/serset
chmod 600 $basePath/Publish/release/serset

#推送到github
docker run -i --rm \
-v $basePath/Publish/release:/root/release serset/git-client bash -c "
set -e
ssh-agent bash -c \"
ssh-add /root/release/serset
ssh -T git@github.com -o StrictHostKeyChecking=no
git config --global user.email 'serset@yeah.com'
git config --global user.name 'lith'
mkdir -p /root/code
cd /root/code
git clone git@github.com:serset/release.git /root/code
mkdir -p /root/code/file/${name}/${name}-${version}
\\cp -rf  /root/release/release-zip/. /root/code/file/${name}/${name}-${version}
git add /root/code/file/${name}/${name}-${version}/.
git commit -m 'auto commit ${version}'
git push -u origin master \" "


 
 
