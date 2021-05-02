#!/bin/bash

CURNT_BRANCH=$(git rev-parse --abbrev-ref HEAD)
DUMMY_BRANCH=${CURNT_BRANCH}_squash_and_merge
git show-ref --verify --quiet "refs/heads/$DUMMY_BRANCH"
if [ $? == 0 ]; then
   echo dummy branch $DUMMY_BRANCH exists
   exit 1
else
   echo ok
fi

# go to root level of current repository:
pushd
cd $(git rev-parse --show-toplevel)

# get initial commit id:
INIT=$(git log --reverse --oneline | head -n 1 | cut -d" " -f1)
git checkout -b "$DUMMY_BRANCH" $INIT
git merge --squash "$CURNT_BRANCH"
git commit -m "Squashed Branch $CURNT_BRANCH at $(date)"
sleep 1
git merge "$CURNT_BRANCH"
git commit -m "Merged Branch $CURNT_BRANCH at $(date)"
git checkout "$CURNT_BRANCH"
git merge "$DUMMY_BRANCH"
# delete the dummy branch, it was only temporarily needed:
git branch -D "$DUMMY_BRANCH"

popd
