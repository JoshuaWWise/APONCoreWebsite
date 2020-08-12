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
        var nameFieldElement = document.getElementById("Tag[" + i + "].name");
        selectOption.text = nameFieldElement.value;
        if (filterText == "" || selectOption.text.toLowerCase().includes(filterText.toLowerCase())) {
            var idField = document.getElementById("Tag[" + i + "].id");
            var idFieldValue = idField.value;

            selectOption.value = idFieldValue;
            selector.appendChild(selectOption);

        }

    }
}

function AddTagToList(sender) {
    var value = sender.value;
    var selectedTags = document.getElementById("selectedTagIndicies").value;

    var TagList = selectedTags.split(',');



    if (!TagList.includes(value)) {
        selectedTags += "," + value;
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
                li.setAttribute("id", ("tagListItemID--" + TagList[i]));
                li.addEventListener("click", removeSelectedTag);
                li.classList.add("APONbtnBlue");
                li.classList.add("APONbtn");
                console.log("Tag_" + TagList[i] + "_Name");
                var TagName = document.getElementsByName("TagID_" + TagList[i] + "_Name")[0].value;
                //var TagName = document.getElementById('Tag[' + TagList[i] + '].name').value;
                li.appendChild(document.createTextNode(TagName));
                ListItem.appendChild(li);
            }
        }
    }
}

function removeSelectedTag(item) {
    var index = item.srcElement.id.split('--')[1];
    var selectedTags = document.getElementById("selectedTagIndicies").value;
 
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
    var newTag = document.getElementById("txtTagFilter").value;

    $.ajax({
        type: "POST",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        url: '../Admin/TagHandler/?handler=tag',
        data: newTag,
        contentType: false,
        processData: false,

        success: function (resp) {
            console.log("stff");
            console.log(resp);
            var containerElement = document.getElementById("hiddenTagInfo");
            containerElement.innerHTML = "";


            var arrayString = "";

            for (var j = 0; j < resp.length; j++) {
                var newEl = document.createElement("input");
                newEl.id = 'Tag[' + j + '].id';
                newEl.type = "hidden";
                newEl.value = resp[j].tagID;

                containerElement.appendChild(newEl);
            }
        }
    });

}