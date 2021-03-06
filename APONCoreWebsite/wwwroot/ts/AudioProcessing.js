var episodeTitle;
var episodeSize;
var episodeDescription;
var webDescription;
var episodeTime;
var tagLocations = new Array();
var rawString;
var charlist;
var charcount;
var determiningEpLength;
var btnAddEpSubmit;
var MBSize;
var KVP = /** @class */ (function () {
    function KVP(k, v) {
        this.key = k;
        this.value = v;
    }
    return KVP;
}());
function startAudioFileProcessing(uploadFile) {
    //Set Submit button to disabled until File Duration is calculated.
    btnAddEpSubmit = document.getElementById("btnAddEpSubmit");
    btnAddEpSubmit.style.visibility = "hidden";
    determiningEpLength = document.getElementById("determiningEpLength");
    MBSize = document.getElementById("fileSizeInMB");
    determiningEpLength.style.visibility = "visible";
    charcount = 0;
    getArrayBuffer(uploadFile).then(function (result) {
        fillInFormData(result);
    });
}
function fillInFormData(ab) {
    var dv = new DataView(ab);
    var bufferSize = 0;
    if (dv.byteLength > 5000) {
        bufferSize = 5000;
    }
    else {
        bufferSize = dv.byteLength - 1;
    }
    rawString = getString(dv, 0, bufferSize);
    episodeSize = dv.byteLength.toString();
    episodeSize = episodeSize.substr(0, episodeSize.length - 2);
    var epsizenum = parseInt(episodeSize);
    var mb = epsizenum / 10240;
    var kb = epsizenum % 102400;
    var episodeSizeText = mb.toString();
    var decplace = episodeSizeText.indexOf('.');
    episodeSizeText = episodeSizeText.substring(0, (decplace + 2));
    document.getElementById("epSize").value = episodeSize;
    MBSize.innerHTML = "(" + episodeSizeText + " MB)";
    //get locations of value pairs for the MP3 ID3 tags.
    getKeyValuePairs();
    //PUT BACK
    episodeTitle = getSection("TIT2");
    var headlineObj = document.getElementById("episodeTitle");
    headlineObj.value = episodeTitle;
    episodeDescription = getSection("COMM");
    document.getElementById("shortDescription").value = episodeDescription;
    document.getElementById("tinyMCETextArea").innerHTML = episodeDescription;
    webDescription = episodeDescription;
    var a = new AudioContext();
    //Set Fields
    getAudioBuffer(ab).then(function (result) {
        getDuration(result, parseInt(episodeSize));
    });
}
function getDuration(aBuff, dvByteLength) {
    var dur = aBuff.duration;
    var hours = Math.floor(dur / 3600);
    dur = dur - hours * 3600;
    var minutes = Math.floor(dur / 60);
    var seconds = Math.floor(dur % 60);
    episodeTime =
        getTimeNum(hours) +
            ":" +
            getTimeNum(minutes) +
            ":" +
            getTimeNum(seconds);
    document.getElementById("epTime").value = episodeTime;
    if (dvByteLength < 1024000) {
        determiningEpLength.style.visibility = "hidden";
        btnAddEpSubmit.style.visibility = "visible";
    }
    else {
        alert("File size over 100MB and therefore too large! Try exporting the file with a lower bit rate to shrink the size.");
        determiningEpLength.innerHTML = "File size over 100MB and therefore too large! Try exporting the file with a lower bit rate to shrink the size.";
    }
}
function getTimeNum(n) {
    if (n < 10) {
        return "0" + n.toString();
    }
    else {
        return n.toString();
    }
}
function getArrayBuffer(f) {
    return f.arrayBuffer();
}
function getAudioBuffer(ab) {
    var a = new AudioContext();
    return a.decodeAudioData(ab);
}
function getString(dv, start, length) {
    for (var i = 0, v = ""; i < length; ++i) {
        v += getChar(dv, start + i);
    }
    return v;
}
function getChar(dv, start) {
    var character = "";
    var ignoreNums = [0, 1, 254, 255];
    if (ignoreNums.indexOf(dv.getUint8(start)) == -1) {
        character = String.fromCharCode(dv.getUint8(start));
        charlist +=
            charcount.toString() +
                ": " +
                character +
                " | " +
                dv.getUint8(start) +
                "\n\r";
        charcount++;
    }
    return character; //+ "|" + dv.getUint8(start) + "|";
}
function getKeyValuePairs() {
    tagLocations = new Array();
    var listOfKeys = "COMM,TCOM,TCON,TPE1,TPE2,TALB,TDAT,TIT1,TIT2,TIT3,TLEN,TPUB,TRCK,TSIZ,TSST,TYER,WCOM";
    var a = listOfKeys.split(",");
    var i = 0;
    for (i = 0; i < a.length; i++) {
        getIndexOf(a[i]);
    }
    var SortedKVP = tagLocations.sort(function (n1, n2) {
        if (n1.value > n2.value) {
            return 1;
        }
        else if (n1.value < n2.value) {
            return -1;
        }
        return 0;
    });
    tagLocations = SortedKVP;
}
function getIndexOf(s) {
    var index = rawString.indexOf(s);
    if (index != -1) {
        tagLocations.push(new KVP(s, index));
    }
}
function getSection(sectionTitle) {
    var index = -1;
    var startIndex = -1;
    var endIndex = -1;
    var i = 0;
    for (i = 0; i < tagLocations.length; i++) {
        if (tagLocations[i].key == sectionTitle) {
            startIndex = tagLocations[i].value + sectionTitle.length + 1;
            try {
                endIndex = tagLocations[i + 1].value;
            }
            catch (e) {
                endIndex = startIndex + 50;
            }
            break;
        }
    }
    var returnString = rawString.substr(startIndex, endIndex - startIndex);
    if (sectionTitle == "COMM" && returnString.substr(0, 3) == "eng") {
        returnString = returnString.substr(3, returnString.length - 3);
    }
    return returnString;
}
