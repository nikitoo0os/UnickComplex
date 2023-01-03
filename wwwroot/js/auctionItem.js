let bid_btn = document.querySelector('#bid_btn')
let remained_time_to_bid = document.querySelector('#remained_time')

console.log();

//if (remained_time_to_bid.textContent.replace(/\s/g, "") == "0") {
//    $(bid_btn).show();
//    $('#remained_time_to_bid').hide()

//    $('#remained_time').hide()
//    $('.auctioneer').hide()
//}

$('#auction_desc_btn').click(() => {
    $('.description_auction_item').show();
    $('.about_auction_item').hide();
    $('.contains_file_auction_item').hide();
});

$('#auction_contains_btn').click(() => {
    $('.contains_file_auction_item').show();
    $('.description_auction_item').hide();
    $('.about_auction_item').hide();
});

$('#auction_about_btn').click(() => {
    $('.about_auction_item').show();
    $('.description_auction_item').hide();
    $('.contains_file_auction_item').hide();

    $('.wrap_container_auction_item').filter = "blur(10px)"
});

$('#bid_btn').click(() => {
    $('.wrap_modal').show();
})

$('#cancel_bid_btn').click(() => {
    $('.wrap_modal').hide();
    
})

$(function () {
    GetStudents();
});

$('#btnSearch').on('click', function (e) {
    var filters = {
        student: $('#student').val(),
        courseId: $('#course').val()
    };
    GetStudents(filters);
});

