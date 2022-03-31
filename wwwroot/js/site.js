
var inputLeft = document.getElementById("input-left");
var inputRight = document.getElementById("input-right");
var minimumValue = document.getElementById("minimum-value");
var maximumValue = document.getElementById("maximum-value");
var thumbLeft = document.querySelector(".slider > .thumb.left");
var thumbRight = document.querySelector(".slider > .thumb.right");
var range = document.querySelector(".slider > .range");
var tempMin = -1;
var tempMax = -1;

function setLeftValue() {
    var _this = inputLeft,

        min = parseInt(_this.min),
        max = parseInt(_this.max);

    _this.value = Math.min(parseInt(_this.value), parseInt(inputRight.value) - 1);

    var percent = ((_this.value - min) / (max - min)) * 100;
    thumbLeft.style.left = percent + "%";
    range.style.left = percent + "%";

    if (tempMin > 0) {
        minimumValue.value = _this.value;
    }

    if (minimumValue.value < _this.min) {
        tempMin = _this.value;
    }
    else {
        minimumValue.value = _this.value;
    }
}
setLeftValue(); function setRightValue() {
    var _this = inputRight,

        min = parseInt(_this.min),
        max = parseInt(_this.max); _this.value = Math.max(parseInt(_this.value), parseInt(inputLeft.value) + 1);

    var percent = ((_this.value - min) / (max - min)) * 100;

    thumbRight.style.right = (100 - percent) + "%";
    range.style.right = (100 - percent) + "%";
    if (tempMax > 0) {
        maximumValue.value = _this.value;
    }

    if (max > _this.min) {
        tempMax = _this.value;
    }
    else {
        maximumValue.value = _this.value;
    }
}
setRightValue();

inputLeft.addEventListener("input", setLeftValue);
inputRight.addEventListener("input", setRightValue);


var inputLeft2 = document.getElementById("input-left2");
var inputRight2 = document.getElementById("input-right2");
var minimumValue2 = document.getElementById("minimum-value2");
var maximumValue2 = document.getElementById("maximum-value2");
var thumbLeft2 = document.querySelector(".slider > .thumb.left2");
var thumbRight2 = document.querySelector(".slider > .thumb.right2");
var range2 = document.querySelector(".slider > .range2");
var tempMin2 = -1;
var tempMax2 = -1;

function setLeft2Value() {
    var _this = inputLeft2,

        min = parseInt(_this.min),
        max = parseInt(_this.max);

    _this.value = Math.min(parseInt(_this.value), parseInt(inputRight2.value) - 1);

    var percent = ((_this.value - min) / (max - min)) * 100;
    thumbLeft2.style.left = percent + "%";
    range2.style.left = percent + "%";

    if (tempMin2 > 0)
    {
        minimumValue2.value = _this.value;
        
    }

    if (minimumValue2.value < _this.min)
    {
        tempMin2 = _this.value;
        
    }
    else
    {
        minimumValue2.value = _this.value;
    }

}
setLeft2Value();

function setRight2Value() {
    var _this = inputRight2,

        min = parseInt(_this.min),
        max = parseInt(_this.max);

    _this.value = Math.max(parseInt(_this.value), parseInt(inputLeft2.value) + 1);

    var percent = ((_this.value - min) / (max - min)) * 100;

    thumbRight2.style.right = (100 - percent) + "%";
    range2.style.right = (100 - percent) + "%";
    if (tempMax2 > 0) {
        maximumValue2.value = _this.value;
    }

    if (max > _this.min) {
        tempMax2 = _this.value;
    }
    else {
        maximumValue2.value = _this.value;
    }
}
setRight2Value();
inputLeft2.addEventListener("input", setLeft2Value);
inputRight2.addEventListener("input", setRight2Value);

