#!/bin/bash

if ! [ -z "$(git status --porcelain)" ]; then
  echo please stash or commit the uncommitted changes first
  exit 1
fi

CURNT_BRANCH=$(git rev-parse --abbrev-ref HEAD)
DUMMY_BRANCH=${CURNT_BRANCH}_squash_and_merge
git show-ref --verify --quiet "refs/heads/$DUMMY_BRANCH"
if [ $? == 0 ]; then
   echo dummy branch $DUMMY_BRANCH exists
   exit 1
fi

# go to root level of current repository:
CURR_DIR=$(pwd)
cd $(git rev-parse --show-toplevel)

# do this anonymously
git config user.name  "Script: $0"
git config user.email "generic.user@donotreply.com"

# get initial commit id:
INIT=$(git log --reverse --oneline | head -n 1 | cut -d" " -f1)
git checkout -b "$DUMMY_BRANCH" $INIT
git merge --squash "$CURNT_BRANCH"
git commit -m "Squashed Branch $CURNT_BRANCH at $(date)"
sleep 1
git merge "$CURNT_BRANCH" -m "Squashed branch '$CURNT_BRANCH'"
git commit -m "Merged Branch $CURNT_BRANCH at $(date)"
git checkout "$CURNT_BRANCH"
git merge "$DUMMY_BRANCH"
# delete the dummy branch, it was only temporarily needed:
git branch -D "$DUMMY_BRANCH"

git config --unset user.name
git config --unset user.email
cd "$CURR_DIR"

echo ""
echo "try1: cd ."
echo "try2: git blame $(git rev-parse --show-toplevel)/Tweak_Log.txt"
echo "try3: git rebase"
echo "NOTE: git rebase will undo what this script did, unless you first try: git push"
