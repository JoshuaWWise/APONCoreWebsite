//Tags


function filterTagList() {
 
    var filterText = document.getElementById("txtTagFilter").value;
    var selector = document.getElementById("TagSelector");

  
    var i = 0;
    var Length = selector.options.length - 1;

  
    for (i = Length; i >= 0; i--) {
    
        selector.remove(i);
    }
    var tagCount = parseInt(document.getElementById("TagModelCount").value);
 
    //Populate the tag console
    for (i = 0; i < tagCount; i++) {
     
     
        var selectOption = document.createElement("OPTION");
      
        var nameFieldElement = document.getElementById("Tag[" + i + "].Name");
      
    
        selectOption.text = nameFieldElement.value;
  
        if (filterText == "" || selectOption.text.toLowerCase().includes(filterText.toLowerCase())) {
        
            var idField = document.getElementById("Tag[" + i + "].Index");
            var idFieldValue = idField.value;

            selectOption.value = i;
            selector.appendChild(selectOption);

        }

    }
}

function AddTagToList(sender) {
    var value = sender.value;
 
    var selectedTags = document.getElementById("selectedTagIndicies").value;
    var selectedTagElement = document.getElementById("Tag[" + value + "].Index");
    var TagList = selectedTags.split(',');
  


    if (!TagList.includes(value)) {
        selectedTags += "," + value;
        document.getElementById("selectedTagIDs").value += "," + selectedTagElement.value;
        document.getElementById("selectedTagIndicies").value = selectedTags;
    }
  
    populateSelectedTagList();


}

function populateSelectedTagList() {
   
    var selectedTags = document.getElementById("selectedTagIndicies").value;
    var TagList = selectedTags.split(",");
    var ListItem = document.getElementById("tagList");

    ListItem.innerHTML = '';
 
    if (TagList.length > 0) {

        for (var i = 0; i < TagList.length; i++) {

           
            if (TagList[i] != "") {
                var li = document.createElement("li");
                li.setAttribute("id", ("tagListIndex--" + TagList[i]));
                li.addEventListener("click", removeSelectedTag);
                li.classList.add("APONbtnBlue");
                li.classList.add("APONbtn");
                
                var TagName = document.getElementById("Tag[" + TagList[i] + "].Name").value;
                
         
                li.appendChild(document.createTextNode(TagName));
                ListItem.appendChild(li);
            }
        }
    }
}

function removeSelectedTag(item) {
    var index = item.srcElement.id.split('--')[1];
    var ID = document.getElementById("Tag[" + index + "].Index").value;
    var selectedTags = document.getElementById("selectedTagIndicies").value;
    var selectedIDS = document.getElementById("selectedTagIDs").value;
    var newSelectedIDS = selectedIDS.replace(ID, "");
    if (newSelectedIDS.substring(0, 1) == ",") {
        if (newSelectedIDS.length > 1) {
            newSelectedIDS = newSelectedIDS.substring(1, newSelectedIDS.length);
        }
        else {
            newSelectedIDS = "";
        }
    }
    document.getElementById("selectedTagIDs").value = newSelectedIDS;
    var newSelectedTags = selectedTags.replace((index + ","), "");
    newSelectedTags = newSelectedTags.replace(index, "");
    if (newSelectedTags.substring(0, 1) == ",") {
        if (newSelectedTags.length > 1) {
            newSelectedTags = newSelectedTags.substring(1, newSelectedTags.length);
        }
        else {
            newSelectedTags = "";
        }
    }
    document.getElementById("selectedTagIndicies").value = newSelectedTags;
  
    populateSelectedTagList();

}



function addNewTag() {
    //Store selected tag ID's before Ajax, then find them again after
    


    var value = document.getElementById("txtTagFilter").value;
  
    $.ajax({
        type: "POST",
    
        url: '../Admin/TagHandler/?handler=tag',
        data: { newTagName:value},
      
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
        },

        success: function (resp) {
           
            var containerElement = document.getElementById("hiddenTagInfo");
            containerElement.innerHTML = "";

            document.getElementById("TagModelCount").value = resp.length;
            document.getElementById("selectedTagIndicies").value = "";
            var selectedIndicies = "";
            var selectedIDS = document.getElementById("selectedTagIDs").value;

            var IDArray = selectedIDS.split(",");
            console.log(IDArray);
            for (var j = 0; j < resp.length; j++) {
                var newEl = document.createElement("input");
              
               
               newEl.setAttribute("id", "Tag[" + j + "].Index");
              
               // newEl.id = id;
                newEl.type = "hidden";
                newEl.value = resp[j].tagID;

                if (IDArray.includes(resp[j].tagID.toString())) {
                    if (selectedIndicies.length == 0) {
                        selectedIndicies = j;
                        console.log(j);
                    }
                    else {
                        selectedIndicies += "," + j;
                        console.log(j);
                    }
                }

                containerElement.appendChild(newEl);

                newEl = document.createElement("input");
                newEl.type = "hidden";
                var nameTagID = "Tag[" + j + "].Name";
                newEl.id = nameTagID;
                
                newEl.value = resp[j].name;
                containerElement.appendChild(newEl);
           
            }

           

            document.getElementById("selectedTagIndicies").value = selectedIndicies;

            document.getElementById("txtTagFilter").value = "";
            filterTagList();
        }
    });

}