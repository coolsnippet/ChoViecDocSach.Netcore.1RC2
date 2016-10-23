<a id="markdown-my-note" name="my-note"></a>
# My note

<!-- TOC insertAnchor:true orderedList:true -->

1. [My note](#my-note)
    1. [Coorporate proxy, ho to find](#coorporate-proxy-ho-to-find)
        1. [1. how to find proxy in coorporate](#1-how-to-find-proxy-in-coorporate)
        2. [2. program: pacparser (in chose this)](#2-program-pacparser-in-chose-this)
        3. [3. SAMPLE (run this)](#3-sample-run-this)
        4. [4. I use this proxy for Visual Studio Code](#4-i-use-this-proxy-for-visual-studio-code)
        5. [5. for ithub windows](#5-for-ithub-windows)
        6. [6. for npm (node package manager so I can install brewer, yeoman to make scaffolding for asp mvc 5 template for dotnet core rc 2)](#6-for-npm-node-package-manager-so-i-can-install-brewer-yeoman-to-make-scaffolding-for-asp-mvc-5-template-for-dotnet-core-rc-2)
        7. [8. sublime](#8-sublime)
        8. [9. bower (package manager for web, downloading dependencies)](#9-bower-package-manager-for-web-downloading-dependencies)

<!-- /TOC -->

<a id="markdown-coorporate-proxy-ho-to-find" name="coorporate-proxy-ho-to-find"></a>
## Coorporate proxy, ho to find 

<a id="markdown-1-how-to-find-proxy-in-coorporate" name="1-how-to-find-proxy-in-coorporate"></a>
### 1. how to find proxy in coorporate
   here:
	http://stackoverflow.com/questions/22368515/how-to-see-the-proxy-settings-on-windows
	says that 
	It's possible to view proxy settings in Google Chrome: chrome://net-internals/#proxy

<a id="markdown-2-program-pacparser-in-chose-this" name="2-program-pacparser-in-chose-this"></a>
### 2. program: pacparser (in chose this)
	http://code.google.com/p/pacparser/

	and release for win32
	https://github.com/pacparser/pacparser/releases

	from here
	http://stackoverflow.com/questions/10508397/how-to-find-proxy-server-and-password-from-pac-file

	and we need install this mingw
	http://www.mingw.org/wiki/getting_started

	2 ways, I chose install using GUI
	and then add to environment PATH the <install folder>\path

	wpad file:
	http://wpad/wpad.dat

	download that file and save to download folder

<a id="markdown-3-sample-run-this" name="3-sample-run-this"></a>
### 3. SAMPLE (run this)

	C:\Users\u330652>C:\Users\u330652\Downloads\pacparser-1.3.3-win32\pacparser-1.3.3-win32\pactester.exe -p C:\Users\u330652\Downloads\wpad.dat -u https://www.goog
	le.com/

	PROXY 10.160.0.45:80; PROXY 10.160.0.47:80; PROXY 10.160.0.46:80; PROXY 10.160.0
	.42:80; PROXY 10.160.0.43:80; PROXY 10.160.0.41:80; PROXY 10.160.0.44:80; PROXY
	10.160.0.40:80; DIRECT


<a id="markdown-4-i-use-this-proxy-for-visual-studio-code" name="4-i-use-this-proxy-for-visual-studio-code"></a>
### 4. I use this proxy for Visual Studio Code
	http://10.160.0.45

<a id="markdown-5-for-ithub-windows" name="5-for-ithub-windows"></a>
### 5. for ithub windows
    from this: http://stackoverflow.com/questions/16153450/github-windows-client-behind-proxy

	Add these entried to your '.gitconfig' file in your user directory (go to %USERPROFILE%):

	[http]
	    proxy = http://<proxy address>:<proxy port>

	[https]
	    proxy = https://<proxy address>:<proxy port>

	so I set 	
	[http]
	    proxy = http://10.160.0.45

<a id="markdown-6-for-npm-node-package-manager-so-i-can-install-brewer-yeoman-to-make-scaffolding-for-asp-mvc-5-template-for-dotnet-core-rc-2" name="6-for-npm-node-package-manager-so-i-can-install-brewer-yeoman-to-make-scaffolding-for-asp-mvc-5-template-for-dotnet-core-rc-2"></a>
### 6. for npm (node package manager so I can install brewer, yeoman to make scaffolding for asp mvc 5 template for dotnet core rc 2)
    from https://jjasonclark.com/how-to-setup-node-behind-web-proxy

	I do: 
	npm config set proxy http://10.160.0.45

	and

	npm config set registry http://registry.npmjs.org/

7. git 
	from http://stackoverflow.com/questions/783811/getting-git-to-work-with-a-proxy-server

	Command to use:
		git config --global http.proxy http://proxyuser:proxypwd@proxy.server.com:8080
	I do:
	git config --global http.proxy http://10.160.0.43:80
	git config --get http.proxy http://10.160.0.43:80

<a id="markdown-8-sublime" name="8-sublime"></a>
### 8. sublime	
	from http://www.bonusbits.com/wiki/HowTo:Add_HTTP_Proxy_to_Package_Control_for_Sublime_Text

	I do:
	Browse Preferences | Package Settings | Package Control | Settings Default
	and add
	"http_proxy": "http://10.160.0.45:80"

<a id="markdown-9-bower-package-manager-for-web-downloading-dependencies" name="9-bower-package-manager-for-web-downloading-dependencies"></a>
### 9. bower (package manager for web, downloading dependencies)
	from http://stackoverflow.com/questions/21705091/bower-behind-a-proxy

	I do:
		create a file name .bowerrc on (%userprofile% folder) by using command line
		copy con .bowerrc
		and add 
		{
    		"proxy":"http://10.160.0.45:80",    		
		}
		and then F6 to save

		
