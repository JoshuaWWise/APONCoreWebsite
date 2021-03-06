﻿let episodeTitle: string;
let episodeSize: string;
let episodeDescription: string;
let webDescription: string;
let episodeTime: string;
let tagLocations = new Array<KVP>();
let rawString: string;
let charlist: string;
let charcount: number;
let determiningEpLength: HTMLElement;
let btnAddEpSubmit: HTMLButtonElement;
let MBSize: HTMLElement;

class KVP {
    key: string;
    value: number;

    constructor(k: string, v: number) {
        this.key = k;
        this.value = v;
    }
}

function startAudioFileProcessing(uploadFile: File) {

    //Set Submit button to disabled until File Duration is calculated.
    btnAddEpSubmit = <HTMLButtonElement>document.getElementById("btnAddEpSubmit");
    btnAddEpSubmit.style.visibility = "hidden";
    determiningEpLength = document.getElementById("determiningEpLength");
    MBSize = document.getElementById("fileSizeInMB");
    determiningEpLength.style.visibility = "visible";

    charcount = 0;
    getArrayBuffer(uploadFile).then((result: any) => {

        fillInFormData(result);
    });



}

function fillInFormData(ab: ArrayBuffer) {


    let dv = new DataView(ab);
    let bufferSize: number = 0;

    if (dv.byteLength > 5000) {
        bufferSize = 5000;
    } else {
        bufferSize = dv.byteLength - 1;
    }

    rawString = getString(dv, 0, bufferSize);
 

    episodeSize = dv.byteLength.toString();
    episodeSize = episodeSize.substr(0, episodeSize.length - 2);
    let epsizenum: number = parseInt(episodeSize);
    let mb: number = epsizenum / 10240;
    let kb: number = epsizenum % 102400;

    let episodeSizeText = mb.toString();
    let decplace = episodeSizeText.indexOf('.');
    episodeSizeText = episodeSizeText.substring(0,(decplace + 2));
    (<HTMLInputElement>document.getElementById("epSize")).value = episodeSize;
    MBSize.innerHTML = "(" + episodeSizeText + " MB)";
    //get locations of value pairs for the MP3 ID3 tags.
    getKeyValuePairs();


    //PUT BACK
    episodeTitle = getSection("TIT2");
    let headlineObj = <HTMLInputElement>document.getElementById("episodeTitle");
    headlineObj.value = episodeTitle;


    episodeDescription = getSection("COMM");

    (<HTMLInputElement>document.getElementById("shortDescription")).value = episodeDescription;
    (<HTMLInputElement>document.getElementById("tinyMCETextArea")).innerHTML = episodeDescription;

    webDescription = episodeDescription;

    let a: AudioContext = new AudioContext();
    //Set Fields
    

    getAudioBuffer(ab).then((result: any) => {

        getDuration(result, parseInt(episodeSize));
    });
}


function getDuration(aBuff: AudioBuffer, dvByteLength: number) {
    let dur: number = aBuff.duration;
    let hours: number = Math.floor(dur / 3600);

 
    dur = dur - hours * 3600;

    let minutes: number = Math.floor(dur / 60);
    let seconds: number = Math.floor(dur % 60);
    episodeTime =
        getTimeNum(hours) +
        ":" +
        getTimeNum(minutes) +
        ":" +
        getTimeNum(seconds);


    (<HTMLInputElement>document.getElementById("epTime")).value = episodeTime;
    if (dvByteLength < 1024000) {
        determiningEpLength.style.visibility = "hidden";
        btnAddEpSubmit.style.visibility = "visible";
    }
    else {
        alert("File size over 100MB and therefore too large! Try exporting the file with a lower bit rate to shrink the size.");
        determiningEpLength.innerHTML = "File size over 100MB and therefore too large! Try exporting the file with a lower bit rate to shrink the size.";
    }

}



function getTimeNum(n: number): string {
    if (n < 10) {
        return "0" + n.toString();
    } else {
        return n.toString();
    }
}
function getArrayBuffer(f: File): Promise<ArrayBuffer> {
    return f.arrayBuffer();
}

function getAudioBuffer(ab: ArrayBuffer): Promise<AudioBuffer> {
    let a: AudioContext = new AudioContext();
    return a.decodeAudioData(ab);
}
function getString(dv: DataView, start: number, length: number) {
    for (var i = 0, v = ""; i < length; ++i) {
        v += getChar(dv, start + i);
    }
    return v;
}

function getChar(dv: DataView, start: number) {
    let character: string = "";
    let ignoreNums: number[] = [0, 1, 254, 255];

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

    tagLocations = new Array<KVP>();

    const listOfKeys: string =
        "COMM,TCOM,TCON,TPE1,TPE2,TALB,TDAT,TIT1,TIT2,TIT3,TLEN,TPUB,TRCK,TSIZ,TSST,TYER,WCOM";

    const a: string[] = listOfKeys.split(",");

    let i: number = 0;
    for (i = 0; i < a.length; i++) {
        getIndexOf(a[i]);
    }

    let SortedKVP: KVP[] = tagLocations.sort(
        (n1: KVP, n2: KVP) => {
            if (n1.value > n2.value) {
                return 1;
            } else if (n1.value < n2.value) {
                return -1;
            }
            return 0;
        }
    );

    tagLocations = SortedKVP;

}

function getIndexOf(s: string): void {
    let index: number = rawString.indexOf(s);
    if (index != -1) {
        tagLocations.push(new KVP(s, index));
    }
}

function getSection(sectionTitle: string): string {
    let index: number = -1;
    let startIndex: number = -1;
    let endIndex: number = -1;
    let i: number = 0;
    for (i = 0; i < tagLocations.length; i++) {
        if (tagLocations[i].key == sectionTitle) {
            startIndex = tagLocations[i].value + sectionTitle.length + 1;
            try {
                endIndex = tagLocations[i + 1].value;
            } catch (e) {
                endIndex = startIndex + 50;
            }
            break;
        }
    }

    let returnString = rawString.substr(startIndex, endIndex - startIndex);



    if (sectionTitle == "COMM" && returnString.substr(0, 3) == "eng") {
        returnString = returnString.substr(3, returnString.length - 3);
    }
    return returnString;
}
