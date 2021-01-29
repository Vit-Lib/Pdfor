FROM centos:centos8.3.2011

RUN yum install libreoffice-writer.x86_64 -y 
RUN yum install libreoffice-calc.x86_64 -y 
RUN yum install libreoffice-impress.x86_64 -y 

RUN yum install libreoffice-langpack-zh-Hans.x86_64 -y 
  
RUN yum groupinstall "Fonts" -y
RUN yum groupinstall "Input Methods" -y
