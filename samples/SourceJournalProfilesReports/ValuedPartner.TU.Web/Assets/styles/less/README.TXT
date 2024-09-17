To work on less files, we suggest to use VS code as the editor.

Use npm to install less and cleancss cli
-	npm install -g less
-	npm install -g clean-css-cli
After changes are made to less file(s)
-	lessc <source less> <destination css file>
-	(e.g lessc .\default-home.less  ..\css\default-home.min.css)
-	To minimize
-	cleancss -o <source css> <destination min css file>
-	(e.g. cleancss -o ..\css\default-home.min.css ..\css\default-home.min.css)
