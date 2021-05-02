#!/bin/bash
#Ändere und ergänze einen Beitrag von einem Fiktiven Nutzer
# Syntax:
#  beitrag.sh user [email]
# Argumente
#  user 	der name des fiktiven benutzers
#  email	die email des fiktiven nutzers
#
#Beschreibung
#  die lokale gitconfig wird angepass mit den gegebenen argumenten
#  die datei Tweak_Log.txt wird in einer zufälligen zeile geändert
#  die Datei Tweak_Log.txt wird mit einer neuen Zeile ergänzt
#  ein git commit wird ausgeführt mit user name und datum als commit message

if [ -z "$1" ]; then
   echo "Syntax: $0 user email"
   echo "Try:    $0 TestUser TestEmail@test.me.com"
   exit 1
fi
if [ -z "$2" ]; then
  EMAIL=$(echo $1 | cut -d" " -f 1)@donotreply.com
  echo "Using: EMAIL=$EMAIL"
else
  EMAIL="$2"
fi


git config user.name "$1"
git config user.email "$EMAIL"

if ! [ -f ../Tweak_Log.txt ]; then
   echo "file ../Tweak_Log.txt must exist!"
   exit 1
fi

let n=$(cat ../Tweak_Log.txt | wc -l)
let nn=$n+1
let n1=$n-3
let n2=$(($RANDOM % $n))
let n3=$n2+1
let n4=$n2+2
echo nn=$nn n3=$n3

DD=$(date)
echo "Neue Zeile $nn von $1 mit email: $EMAIL am $DD" >> ../Tweak_Log.txt
head -n $n2 ../Tweak_Log.txt >> qwe.$$
echo "Changed Line $n3 by $1" >> qwe.$$
tail -n +$n4 ../Tweak_Log.txt >> qwe.$$
mv qwe.$$ ../Tweak_Log.txt

git commit -a -m "$1 von $DD"
