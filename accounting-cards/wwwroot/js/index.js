const app = Vue.createApp({
    data(){
        return{
            user:{
                account: "",
                login:{
                    account: "",
                    password: "",
                },
                register:{
                    account: "",
                    password: "",
                    name: "",
                    key: ""
                }
            }
        }
    },
    methods:{
        postCheck: async function () {
            let vm = this;

            if (isNullOrEmpty(vm.user.account)) {
                $('#account').addClass('error-required');
                $('#account').addClass('error-shake');
                await sleep(600);
                $('#account').removeClass('error-shake');
                return;
            }

            let data = {
                Account: vm.user.account,
            };

            axios({
                method: 'post',
                url: './api/user/session',
                data: data
            }).then(res => {
                vm.user.register.key = res.data.salt
                let step = res.data.step;
                let account = res.data.account;
                switch (step) {
                    case 0:
                        vm.user.register.account = account;
                        $('#form-check').addClass('display-none-form');
                        $('#form-register').removeClass('display-none-form');
                        return;
                    case 1:
                        vm.user.login.account = account;
                        $('#form-check').addClass('display-none-form');
                        $('#form-login').removeClass('display-none-form');
                        return;
                }
            }).catch(err => {
                console.log(err.response);
                // alert_fail(err.response.data, `嘿！${vm.user.login.account}，你可能記錯了帳號或密碼`)
            })
        },
        postRegister: async function () {
            let vm = this;

            if (isNullOrEmpty(vm.user.register.password)) {
                $('#register-password').addClass('error-shake');
                $('#register-password').addClass('error-required');
                await sleep(600);
                $('#register-password').removeClass('error-shake');
                return;
            }

            if (isNullOrEmpty(vm.user.register.name)) {
                $('#register-name').addClass('error-shake');
                $('#register-name').addClass('error-required');
                await sleep(600);
                $('#register-name').removeClass('error-shake');
                return;
            }

            let data = {
                Account: vm.user.register.account,
                Password: toSHA256(vm.user.register.password, vm.user.register.key),
                Name: vm.user.register.name
            };

            axios({
                method: 'post',
                url: './api/user/new',
                data: data
            }).then(res => {
                alert_success('註冊成功', '感謝你的註冊！重新整理回到登入頁面號馬上開始使用吧', true);
            }).catch(err => {
                alert_fail(`ERROR：${err.response.status}`, `嘿！${err.response.data}`)
            })
        },
        postLogin: function () {
            let vm = this;
            let data = {
                Account: vm.user.login.account,
                Password: vm.user.login.password
            };

            axios({
                method: 'post',
                url: './api/user',
                data: data
            }).then(res => {
                alert_success("歡迎登入", `嗨！${vm.user.login.account}，很高興看到你回來`, false);
                localStorage.setItem("userInfo", JSON.stringify(res.data));
            }).catch(err => {
                alert_fail(err.response.data, `嘿！${vm.user.login.account}，你可能記錯了帳號或密碼`)
            })
        },
    },
});
app.mount('#app');