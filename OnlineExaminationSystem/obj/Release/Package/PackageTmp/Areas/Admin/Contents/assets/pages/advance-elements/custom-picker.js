"use strict";
$(document).ready(function () {


    // Date-dropper js start

    $("#dropper-default").dateDropper({
        dropWidth: 200,
        dropPrimaryColor: "#1abc9c",
        dropBorder: "1px solid #1abc9c"
    }),
        $("#dropper-animation").dateDropper({
            dropWidth: 200,
            init_animation: "bounce",
            dropPrimaryColor: "#1abc9c",
            dropBorder: "1px solid #1abc9c"
        }),
        $("#dropper-format").dateDropper({
            dropWidth: 200,
            format: "F S, Y",
            dropPrimaryColor: "#1abc9c",
            dropBorder: "1px solid #1abc9c"
        }),
        $("#dropper-lang").dateDropper({
            dropWidth: 200,
            format: "F S, Y",
            dropPrimaryColor: "#1abc9c",
            dropBorder: "1px solid #1abc9c",
            lang: "ar"
        }),
        $("#dropper-lock").dateDropper({
            dropWidth: 200,
            format: "F S, Y",
            dropPrimaryColor: "#1abc9c",
            dropBorder: "1px solid #1abc9c",
            lock: "from"
        }),
        $("#dropper-max-year").dateDropper({
            dropWidth: 200,
            dropPrimaryColor: "#1abc9c",
            dropBorder: "1px solid #1abc9c",
            maxYear: "2020"
        }),
        $("#dropper-min-year").dateDropper({
            dropWidth: 200,
            dropPrimaryColor: "#1abc9c",
            dropBorder: "1px solid #1abc9c",
            minYear: "1990"
        }),
        $("#year-range").dateDropper({
            dropWidth: 200,
            dropPrimaryColor: "#1abc9c",
            dropBorder: "1px solid #1abc9c",
            yearsRange: "5"
        }),
        $("#dropper-width").dateDropper({
            dropPrimaryColor: "#1abc9c",
            dropBorder: "1px solid #1abc9c",
            dropWidth: 500
        }),
        $(".dropper-dangercolor").dateDropper({
            dropWidth: 200,
            dropPrimaryColor: "#42a5f5",
            dropBorder: "1px solid #42a5f5",
        }),
        $("#dropper-backcolor").dateDropper({
            dropWidth: 200,
            dropPrimaryColor: "#1abc9c",
            dropBorder: "1px solid #1abc9c",
            dropBackgroundColor: "#bdc3c7"
        }),
        $("#dropper-txtcolor").dateDropper({
            dropWidth: 200,
            dropPrimaryColor: "#46627f",
            dropBorder: "1px solid #46627f",
            dropTextColor: "#FFF",
            dropBackgroundColor: "#e74c3c"
        }),
        $("#dropper-radius").dateDropper({
            dropWidth: 200,
            dropPrimaryColor: "#1abc9c",
            dropBorder: "1px solid #1abc9c",
            dropBorderRadius: "0"
        }),
        $("#dropper-border").dateDropper({
            dropWidth: 200,
            dropPrimaryColor: "#1abc9c",
            dropBorder: "2px solid #1abc9c"
        }),
        $(".dropper-shadow").dateDropper({
            dropWidth: 200,
            dropPrimaryColor: "#1abc9c",
            dropBorder: "1px solid #1abc9c",
            dropBorderRadius: "20px",
            dropShadow: "0 0 20px 0 rgba(26, 188, 156, 0.6)"
        })
    // Date-dropper js end
});
