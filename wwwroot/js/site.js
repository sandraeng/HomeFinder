
var inputLeft = document.getElementById("input-left");
var inputRight = document.getElementById("input-right");
var minimumValue = document.getElementById("minimum-value");
var maximumValue = document.getElementById("maximum-value");
var thumbLeft = document.querySelector(".slider > .thumb.left");
var thumbRight = document.querySelector(".slider > .thumb.right");
var range = document.querySelector(".slider > .range");

function setLeftValue() {
    var _this = inputLeft,

        min = parseInt(_this.min),
        max = parseInt(_this.max);

    _this.value = Math.min(parseInt(_this.value), parseInt(inputRight.value) - 1);

    var percent = ((_this.value - min) / (max - min)) * 100;
    thumbLeft.style.left = percent + "%";
    range.style.left = percent + "%";
    minimumValue.value = _this.value;
}
setLeftValue(); function setRightValue() {
    var _this = inputRight,

        min = parseInt(_this.min),
        max = parseInt(_this.max); _this.value = Math.max(parseInt(_this.value), parseInt(inputLeft.value) + 1);

    var percent = ((_this.value - min) / (max - min)) * 100;

    thumbRight.style.right = (100 - percent) + "%";
    range.style.right = (100 - percent) + "%";
    maximumValue.value = _this.value;
}
setRightValue();

inputLeft.addEventListener("input", setLeftValue);
inputRight.addEventListener("input", setRightValue);


var inputLeft = document.getElementById("input-left2");
var inputRight = document.getElementById("input-right2");
var minimumValue = document.getElementById("minimum-value2");
var maximumValue = document.getElementById("maximum-value2");
var thumbLeft = document.querySelector(".slider > .thumb.left2");
var thumbRight = document.querySelector(".slider > .thumb.right2");
var range = document.querySelector(".slider > .range2");

function setLeftValue() {
    var _this = inputLeft,

        min = parseInt(_this.min),
        max = parseInt(_this.max);

    _this.value = Math.min(parseInt(_this.value), parseInt(inputRight.value) - 1);

    var percent = ((_this.value - min) / (max - min)) * 100;
    thumbLeft.style.left = percent + "%";
    range.style.left = percent + "%";
    minimumValue.value = _this.value;
}
setLeftValue(); function setRightValue() {
    var _this = inputRight,

        min = parseInt(_this.min),
        max = parseInt(_this.max); _this.value = Math.max(parseInt(_this.value), parseInt(inputLeft.value) + 1);

    var percent = ((_this.value - min) / (max - min)) * 100;

    thumbRight.style.right = (100 - percent) + "%";
    range.style.right = (100 - percent) + "%";
    maximumValue.value = _this.value;
}
setRightValue(); inputLeft.addEventListener("input", setLeftValue);
inputRight.addEventListener("input", setRightValue);

