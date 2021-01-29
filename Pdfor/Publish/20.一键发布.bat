:: @echo off

cd /d 脚本\发布脚本 

:: 并行发布
:: for /R %%s in (发布-*) do (   
::  start "发布" "%%s"
:: )  

:: 串行发布
for /R %%s in (发布-*) do (   
 call "%%s"
)  

cd /d ..\制作docker镜像
call "docker镜像-1.复制文件.bat"

echo 发布完成
echo 发布完成
echo 发布完成

:: pause