<!-- TOC insertAnchor:true orderedList:true -->

1. [Look for an element](#look-for-an-element)
    1. [1. select/find a element/node contains value](#1-selectfind-a-elementnode-contains-value)
2. [Modify value of an element](#modify-value-of-an-element)

<!-- /TOC -->

<a id="markdown-look-for-an-element" name="look-for-an-element"></a>
# Look for an element

<a id="markdown-1-selectfind-a-elementnode-contains-value" name="1-selectfind-a-elementnode-contains-value"></a>
## 1. select/find a element/node contains value
from http://stackoverflow.com/questions/3844208/html-agility-pack-find-comment-node
e.g I want to look for a comment
htmlDoc.DocumentNode.SelectSingleNode("//comment()[contains(., 'Buying Options')]/following-sibling::script")

<a id="markdown-modify-value-of-an-element" name="modify-value-of-an-element"></a>
# Modify value of an element
