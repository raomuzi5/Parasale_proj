var gvMy;
var gvMy2;
var TTourShadow = 'ttour-shadow';
var OVERLAY = 'ttour-overlay';
var WRAPPER = 'ttour-wrapper';
var TIP = 'ttour-tip';
var elementPosition;
var getElement;
var createTip;
var setPosition;
var el;


window.Tour = (function (my) {

    var btnEve;
    self = this;

    my.prototype.init = function (options) {
        this.current = 0;
        set.call(this, defaults(), options);
    };

    my.prototype.override = function (name, newMethod) {
        var method = this[name];
        this[name] = newMethod.bind(this, method.bind(this))
    };

    my.prototype.start = function () {
        this.showOverlay();
        var GetStep = getSessionStorageValue("step");
        //this.showStep(this.steps[this.current = 0]);
        //this.showStep(this.steps[this.current = this.steps.step]);
        for (var i = 0; i < this.steps.length; i++) {
            if (this.steps[i].step == GetStep) {
                debugger
                this.showStep(this.steps[i]);
            }
        }
    };
    my.prototype.directstart = function () {
        //this.showOverlay();
        this.showStep(this.steps[this.current = 0]);
    };

    my.prototype.goToStep = function (index) {
        this.current = index;
        this.showStep(this.steps[this.current]);
    };

    my.prototype.nextStep = function () {
        synths.cancel();
        var chkStep = getSessionStorageValue(IsSessionDirect);
        this.steps = datas;
        var step;
        if (chkStep === 'direct') {
            btnEve = chkStep;
            step = this.steps[0];
            !!step ? this.showStep(step) : this.end();
        }
        else {
            btnEve = "next";
            step = this.steps[++this.current];
            if (step !== undefined) {
                var StepNumber = step.step;
                createSession(StepNumber, true, "");
                !!step ? this.showStep(step) : this.end();
            }
            else {
                this.end();
                endRecognitions();
            }
        }
        if (currentStep === "step=45") {
            finishTour();
        } else {
            removeKeyFromSessionStorage(IsSessionDirect);
        }
        doSpeechFromToursJs(step.description)
        ++stepLevel;
        setSessionStorageItem(StepName, step.title)
    };
    var GetStepNo = function () {
        var GetStep = getSessionStorageValue(SessionStep);
        removeSessionValues();
        createSession(GetStep, true, "");
    };
    my.prototype.prevStep = function () {
        synths.cancel();
        btnEve = "prev";
        PrevNext(btnEve);
        var step = this.steps[Math.max(--this.current, 0)];
        this.showStep(step);
        doSpeechFromToursJs(step.description);
        --stepLevel;
    };
    var PlayVoice = function () {
        setTimeout(function () {
            synths.speak(msg);
        }, 200);
    }

    my.prototype.showStep = function (step) {

        //
        //goToByScroll('.theme-purple');
        GoToTop();
        var position = elementPosition(step.element, this.container, this.padding);
        //var Shadow = getElement(this, '.' + TTourShadow);
        var wrapper = getElement(this, '.' + WRAPPER);
        var tip = getElement(this, '.' + TIP);
        if (!!tip) {
            wrapper.removeChild(tip);
        }
        tip = createTip.call(this, step, step.position || "bottom");
        wrapper.appendChild(tip);
        setPosition(getElement(this, '.' + OVERLAY), position);
        appendHtml();
        var chkStep = getSessionStorageValue(IsSessionDirect);
        if (chkStep == 'direct') {
            $(".next").addClass('Hidden');
            $(".prev").addClass('Hidden');
        }
        doSpeechFromToursJs(step.message)
        //if (step.element != '.body') {
        //    goToByScroll(step.element);
        //}
        //else if (step.element != '.practiceObjections') {
        //    setTimeout(function () {
        //        GoToTop();
        //        goToByScroll(step.element);
        //    }, 3000);
        //}
        //}, 3000);
    };
    // This is a functions that scrolls to #{blah}link

    my.prototype.showOverlay = function () {
        debugger
        this.el = this.el || createShadow.call(this);
        this.container.appendChild(this.el);
    };

    my.prototype.end = function () {
        this.container.removeChild(this.el);
        this.el = null;
    };

    getElement = function (self, selector) {
        return self.el.querySelector(selector);
    };

    var createShadow = function () {
        if ($('.ttour-shadow').length > 0) {
            $('.ttour-shadow').remove();
            return newElement("div", {
                className: "ttour-shadow",
                //onclick: this.end.bind(this)
            }, [
                createOverlay()
            ]);
        }
        else {
            return newElement("div", {
                className: "ttour-shadow",
                //onclick: this.end.bind(this)
            }, [
                createOverlay()
            ]);
        }
       
    };

    var createOverlay = function () {
        var wrapper = newElement("div", {
            className: WRAPPER
        });

        return newElement("div", {
            className: OVERLAY
        }, [
            wrapper
        ]);
    };

    setPosition = function (el, position) {
        el.style.left = position.left + "px";
        el.style.top = position.top + "px";
        el.style.width = position.width + "px";
        el.style.height = position.height + "px";
    };

    var createArrow = function () {
        return newElement("div", {
            className: "ttour-arrow"
        });
    };

    var createTip = function (step, classes) {
        return newElement("div", {
            className: TIP + " tip-" + this.current + " " + classes,
            style: { position: 'absolute' }
            //,
            //onclick: function (e) {
            //    e.stopPropagation();
            //}
        }, [
            tipHeader(step.title),
            tipBody(step.description),
            tipFooter.call(this),
            createArrow()
        ]);
    };

    var tipHeader = function (title) {
        return newElement("div", {
            className: "ttour-header"
        }, [
            tipTitle(title)
        ]);
    };

    var tipBody = function (description) {
        return newElement("div", {
            className: "ttour-body",
            innerHTML: description
        });
    };



    var createBullets = function (totalSteps, current) {
        var children = [];
        for (var i = 0; i < totalSteps; i++) {
            children.push(createBullet(i === current));
        }

        return newElement("div", {
            className: "ttour-bullets text-center"
        }, children);
    };

    var createBullet = function (active) {
        return newElement("div", {
            className: "ttour-bullet " + (active ? 'active' : '')
        });
    };

    var nextButton = function (last) {

        return newElement("button", {
            className: "next btn btn-primary",
            innerText: last ? this.done : this.next,
            onclick: this.nextStep.bind(this)

        });
    };

    var prevButton = function () {

        return newElement("button", {
            className: "prev btn btn-primary",
            innerText: this.prev,
            onclick: this.prevStep.bind(this)
        });

    };

    var tipFooter = function () {
        var children = [

            nextButton.call(this, (this.steps.length - 1) === this.current),
            createBullets(this.steps.length, this.current),

        ];

        if (this.current > 0)
            children.unshift(prevButton.call(this));

        var tfoot = newElement("div", {
            className: "ttour-footer"
        }, children);
        return tfoot;
    };

    var tipTitle = function (titleText) {
        return newElement("h1", {
            innerText: titleText
        });
    };

    elementPosition = function (element, parentEl, padding) {
        // Default to the parentEl if we can't find the element. This should be obvious enough
        // to the caller that the element was not able to be found.
        el = parentEl.querySelector(element) || parentEl;
        var position = el.getBoundingClientRect();
        var topPos;
        if (element == '.CollectionName') {
            topPos = ((parentEl.scrollTop + position.top) - padding) * 2;
        }
        else {
            topPos = (parentEl.scrollTop + position.top) - padding;
        }
        return {
            left: (parentEl.scrollLeft + position.left) - padding,
            top: topPos,
            width: position.width + (padding * 2),
            height: position.height + (padding * 2)
        };
    };

    var newElement = function (tag, attributes, children) {
        var el = document.createElement(tag);
        set.call(el, attributes);
        for (var i = 0; i < (children || []).length; i++) {
            el.appendChild(children[i]);
        }
        return el;
    };

    var defaults = function () {
        return {
            steps: [],
            padding: 3,
            container: document.body,
            next: "Next",
            done: "Done",
            prev: "Prev"
        };
    };

    var set = function () {
        var self = this;
        gvMy2 = self;
        for (var i = 0; i < arguments.length; i++) {
            var keys = Object.keys(arguments[i]);
            for (var j = 0; j < keys.length; j++) {
                self[keys[j]] = arguments[i][keys[j]];
            }
        }
    };
    //gvMy = my;
    //gvMy2 = this;
    return my;


})(window.Tour || function (options) { this.init(options || {}) })
function PrevNext(btnEve) {

    var subStr = currentStep.match("step=(.*)");
    subStr = parseInt(subStr[1]);
    if (btnEve !== undefined) {
        if (btnEve === "next") {
            subStr = subStr + 1;
        }
        else if (btnEve == "direct") {
            subStr = subStr;
        }
        else {
            subStr = subStr - 1;
        }
        btnEve = undefined;
        // eraseCookie("username");
        //createCookie(subStr);
        removeSessionValues();
        createSession(subStr, false, "");
        checkSteps();

    }

}
function DirectInput(stepnumber) {
    synths.cancel();
    btnEve = "direct";
    var ClickedStep = Direct(btnEve, stepnumber);
    var step = datas[0];
    tour = window.tour;
    //!!step ? window.tour.showStep(step) : gvMy2.end();
    !!step ? DirectshowStep(step) : gvMy2.end();
    if (ClickedStep === "step=45") {
        finishTour();
    }

}
function Direct(btnEve, Step) {
    var subStr;
    if (btnEve !== undefined) {
        if (btnEve === "direct") {
            subStr = Step;
        }
        else {
            subStr = subStr - 1;
        }
        btnEve = undefined;
        eraseCookie("username");
        createCookie(subStr, false);
        checkSteps();
        return subStr;
    }

}
DirectshowStep = function (step) {

    var position = elementPosition(step.element, window.tour.container, window.tour.padding);
    var wrapper = getElement(window.tour, '.' + WRAPPER);
    var tip = getElement(window.tour, '.' + TIP);
    if (!!tip) {
        wrapper.removeChild(tip);
    }
    tip = createTip.call(window.tour, step, step.position || "bottom");
    wrapper.appendChild(tip);
    setPosition(getElement(window.tour, '.' + OVERLAY), position);

    if ($(".ttour-footer .skip").length > 0) {
        //do nothing
    }

    else {
        appendHtml();

    }
    clickevent();
};
var goToByScroll = function (className) {
    // Scroll
    $('html,body').animate({
        scrollTop: $(className).offset().top
    }, 'slow');
}
var GoToTop = function (className) {
    window.scrollTo(0, 0);
}
//





