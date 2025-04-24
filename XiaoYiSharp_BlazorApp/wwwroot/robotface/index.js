function createEyeController(elements = {}, eyeSize = '33.33vmin') {
    let _eyeSize = eyeSize;
    let _blinkTimeoutID = null;

    let {
        leftEye,
        rightEye,
        upperLeftEyelid,
        upperRightEyelid,
        lowerLeftEyelid,
        lowerRightEyelid,
    } = elements;

    function setElements({
        leftEye: newLeftEye,
        rightEye: newRightEye,
        upperLeftEyelid: newUpperLeftEyelid,
        upperRightEyelid: newUpperRightEyelid,
        lowerLeftEyelid: newLowerLeftEyelid,
        lowerRightEyelid: newLowerRightEyelid,
    } = {}) {
        leftEye = newLeftEye;
        rightEye = newRightEye;
        upperLeftEyelid = newUpperLeftEyelid;
        upperRightEyelid = newUpperRightEyelid;
        lowerLeftEyelid = newLowerLeftEyelid;
        lowerRightEyelid = newLowerRightEyelid;
    }

    function _createKeyframes({
        tgtTranYVal = 0,
        tgtRotVal = 0,
        enteredOffset = 1 / 3,
        exitingOffset = 2 / 3,
    } = {}) {
        return [
            { transform: `translateY(0px) rotate(0deg)`, offset: 0.0 },
            { transform: `translateY(${tgtTranYVal}) rotate(${tgtRotVal})`, offset: enteredOffset },
            { transform: `translateY(${tgtTranYVal}) rotate(${tgtRotVal})`, offset: exitingOffset },
            { transform: `translateY(0px) rotate(0deg)`, offset: 1.0 },
        ];
    }

    function express({
        type = '',
        duration = 1000,
        enterDuration = 75,
        exitDuration = 75,
    }) {
        if (!leftEye) {
            console.warn('Eye elements are not set; return;');
            return;
        }

        const options = { duration };

        switch (type) {
            case 'happy':
                return {
                    lowerLeftEyelid: lowerLeftEyelid.animate(_createKeyframes({
                        tgtTranYVal: `calc(${_eyeSize} * -2 / 3)`,
                        tgtRotVal: `30deg`,
                        enteredOffset: enterDuration / duration,
                        exitingOffset: 1 - (exitDuration / duration),
                    }), options),
                    lowerRightEyelid: lowerRightEyelid.animate(_createKeyframes({
                        tgtTranYVal: `calc(${_eyeSize} * -2 / 3)`,
                        tgtRotVal: `-30deg`,
                        enteredOffset: enterDuration / duration,
                        exitingOffset: 1 - (exitDuration / duration),
                    }), options),
                };

            case 'sad':
                return {
                    upperLeftEyelid: upperLeftEyelid.animate(_createKeyframes({
                        tgtTranYVal: `calc(${_eyeSize} * 1 / 3)`,
                        tgtRotVal: `-20deg`,
                        enteredOffset: enterDuration / duration,
                        exitingOffset: 1 - (exitDuration / duration),
                    }), options),
                    upperRightEyelid: upperRightEyelid.animate(_createKeyframes({
                        tgtTranYVal: `calc(${_eyeSize} * 1 / 3)`,
                        tgtRotVal: `20deg`,
                        enteredOffset: enterDuration / duration,
                        exitingOffset: 1 - (exitDuration / duration),
                    }), options),
                };

            case 'angry':
                return {
                    upperLeftEyelid: upperLeftEyelid.animate(_createKeyframes({
                        tgtTranYVal: `calc(${_eyeSize} * 1 / 4)`,
                        tgtRotVal: `30deg`,
                        enteredOffset: enterDuration / duration,
                        exitingOffset: 1 - (exitDuration / duration),
                    }), options),
                    upperRightEyelid: upperRightEyelid.animate(_createKeyframes({
                        tgtTranYVal: `calc(${_eyeSize} * 1 / 4)`,
                        tgtRotVal: `-30deg`,
                        enteredOffset: enterDuration / duration,
                        exitingOffset: 1 - (exitDuration / duration),
                    }), options),
                };

            case 'focused':
                return {
                    upperLeftEyelid: upperLeftEyelid.animate(_createKeyframes({
                        tgtTranYVal: `calc(${_eyeSize} * 1 / 3)`,
                        enteredOffset: enterDuration / duration,
                        exitingOffset: 1 - (exitDuration / duration),
                    }), options),
                    upperRightEyelid: upperRightEyelid.animate(_createKeyframes({
                        tgtTranYVal: `calc(${_eyeSize} * 1 / 3)`,
                        enteredOffset: enterDuration / duration,
                        exitingOffset: 1 - (exitDuration / duration),
                    }), options),
                    lowerLeftEyelid: lowerLeftEyelid.animate(_createKeyframes({
                        tgtTranYVal: `calc(${_eyeSize} * -1 / 3)`,
                        enteredOffset: enterDuration / duration,
                        exitingOffset: 1 - (exitDuration / duration),
                    }), options),
                    lowerRightEyelid: lowerRightEyelid.animate(_createKeyframes({
                        tgtTranYVal: `calc(${_eyeSize} * -1 / 3)`,
                        enteredOffset: enterDuration / duration,
                        exitingOffset: 1 - (exitDuration / duration),
                    }), options),
                };

            case 'confused':
                return {
                    upperRightEyelid: upperRightEyelid.animate(_createKeyframes({
                        tgtTranYVal: `calc(${_eyeSize} * 1 / 3)`,
                        tgtRotVal: `-10deg`,
                        enteredOffset: enterDuration / duration,
                        exitingOffset: 1 - (exitDuration / duration),
                    }), options),
                };

            default:
                console.warn(`Invalid input type=${type}`);
        }
    }

    function blink({ duration = 150 } = {}) {
        if (!leftEye) {
            console.warn('Eye elements are not set; return;');
            return;
        }

        [leftEye, rightEye].forEach((eye) => {
            eye.animate([
                { transform: 'rotateX(0deg)' },
                { transform: 'rotateX(90deg)' },
                { transform: 'rotateX(0deg)' },
            ], {
                duration,
                iterations: 1,
            });
        });
    }

    function startBlinking({ maxInterval = 5000 } = {}) {
        if (_blinkTimeoutID) {
            console.warn(`Already blinking with timeoutID=${_blinkTimeoutID}; return;`);
            return;
        }
        const blinkRandomly = (timeout) => {
            _blinkTimeoutID = setTimeout(() => {
                blink();
                blinkRandomly(Math.random() * maxInterval);
            }, timeout);
        };
        blinkRandomly(Math.random() * maxInterval);
    }

    function stopBlinking() {
        clearTimeout(_blinkTimeoutID);
        _blinkTimeoutID = null;
    }

    function setEyePosition(eyeElem, x, y, isRight = false) {
        if (!eyeElem) {
            console.warn('Invalid inputs ', eyeElem, x, y, '; returning');
            return;
        }

        if (!!x) {
            if (!isRight) {
                eyeElem.style.left = `calc(${_eyeSize} / 3 * 2 * ${x})`;
            } else {
                eyeElem.style.right = `calc(${_eyeSize} / 3 * 2 * ${1 - x})`;
            }
        }
        if (!!y) {
            eyeElem.style.bottom = `calc(${_eyeSize} / 3 * 2 * ${1 - y})`;
        }
    }

    return {
        setElements,
        express,
        blink,
        startBlinking,
        stopBlinking,
        setEyePosition,
    };
}

window.eyes = createEyeController({
    leftEye: document.querySelector('.left.eye'),
    rightEye: document.querySelector('.right.eye'),
    upperLeftEyelid: document.querySelector('.left .eyelid.upper'),
    upperRightEyelid: document.querySelector('.right .eyelid.upper'),
    lowerLeftEyelid: document.querySelector('.left .eyelid.lower'),
    lowerRightEyelid: document.querySelector('.right .eyelid.lower'),
});
