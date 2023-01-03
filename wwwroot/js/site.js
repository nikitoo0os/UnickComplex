let passInput = document.querySelector('#el-pass')
let passConfirmInput = document.querySelector('#el-pass1')
let submitBtn = document.querySelector('#el-btn')
let btn = document.querySelector('.wrap__auth')

$(passConfirmInput).on("keyup", function () { 

	var value_input1 = $(passInput).val(); 
	var value_input2 = $(this).val(); 

	if (value_input1 != value_input2) { 

		$(".error").html("Пароли не совпадают!"); 
		$(submitBtn).attr("disabled", "disabled"); 

	} else { 

		$(submitBtn).removeAttr("disabled");  
		$(".error").html(""); 
	}
});

