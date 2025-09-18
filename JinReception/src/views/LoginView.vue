<script setup>
import { reactive } from 'vue'
import { useRouter } from 'vue-router'
import { mdiAccount, mdiAsterisk } from '@mdi/js'
import SectionFullScreen from '@/components/SectionFullScreen.vue'
import CardBox from '@/components/CardBox.vue'
import FormCheckRadio from '@/components/FormCheckRadio.vue'
import FormField from '@/components/FormField.vue'
import FormControl from '@/components/FormControl.vue'
import BaseButton from '@/components/BaseButton.vue'
import BaseButtons from '@/components/BaseButtons.vue'
import LayoutGuest from '@/layouts/LayoutGuest.vue'

const form = reactive({
  login: 'john.doe',
  pass: 'highly-secure-password-fYjUw-',
  remember: true,
})

const router = useRouter()

const submit = () => {
  router.push('/dashboard')
}
</script>

<template>
  <LayoutGuest>
    <section class="bg-gray-800 h-screen pt-[10%] relative items-center justify-center">
            <div class="top-blue w-[250px] h-[250px] bg-blue-400 rounded-full absolute top-[10%] left-[50%]"></div>
            <div class="bottom-pink w-[280px] h-[280px] rounded-full absolute top-[50%] left-[12%] lg:left-[30%]"></div>
            <div class="top-orange w-[300px] h-[300px] rounded-full absolute top-[5%] left-[5%] md:left-[23%] lg:left-[30%]"></div>
            <div class="container w-[350px] sm:w-[350px] m-auto text-center p-8 text-white z-10" style="backdrop-filter: blur(20px);">
                    <img 
                      id="passport" 
                      src="https://upload.wikimedia.org/wikipedia/commons/6/67/User_Avatar.png" 
                      alt=""
                      class="mx-auto"  
                    >
                    <p> <span class="text-xl sm:text-2xl">Login Here</span></p>
                
                <hr>
                <form action="POST">
                    <input type="text" id="username" placeholder="Username..." class="w-full mx-auto text-base sm:textlg">
                    <input type="password" id="password" placeholder="Password..." class="w-full mx-auto text-base sm:textlg">
                    <button 
                      type="submit"
                      class="p-2 sm:text-lg bg-blue-500 rounded-2xl m-8 w-36 mx-auto sm:w-48 hover:bg-gradient-to-r hover:from-orange-500 hover:via-pink-500 hover:to-pink-700"
                    >
                    Login
                    </button>            

                </form>
                <p>If you're new here, click to <a href="#" class="underline hover:text-pink-300">SignUp</a></p>             
            </div>
    
    </section>
  </LayoutGuest>
</template>

<script>
  // ...
</script>

<style>

.container{
    -webkit-backdrop-filter: blur(20px);
    border-radius: 20px;
    position: relative;
    z-index: 10;
    border-bottom: 5px solid rgba(255, 255, 255, 0.3);
    border-left: 5px solid rgba(255, 255, 255, 0.3);
}

.container:hover{
    transform-origin: 20px;
    transition: cubic-bezier(0.215, 0.610, 0.355, 1);
    transition: ease-in-out;
    box-shadow: #035711;
}


.your-name, article, h1{
    background-color: rgba(0, 0, 0, 0);
}
.your-name{
    display: flex;
    justify-content: space-between;
}




p{
    background-color: rgba(0, 0, 0, 0);
}
.name-u{
    background-color: rgba(0, 0, 0, 0.1);
    margin-bottom: 30px;
    font-size: 25px;
    font-weight: normal;
    line-height: 40px;
    font-family: Arial, Helvetica, sans-serif;
}

 #passport{
    width: 120px;
    border-radius: 50%;
    border: 5px solid white;
    margin-bottom: 20px;
    animation: passport 5s linear 4s infinite alternate none running;
}
input{
    width: 90%;
    padding: 10px;
    border-radius: 10px;
    margin: 10px;
    background-color: rgba(0, 0, 0, 0.3);
    color: #ffffff;
    font-size: 22px;
}
form, .form-container{
    background-color: rgba(0, 0, 0, .1);
}
input:focus{
    background-color: rgb(0, 0, 0, 0);
    color: rgb(255, 255, 255);
    font-style: italic;
}

.bottom-pink{
    background: linear-gradient(-45deg, #753a88, #cc2b5e);
    animation: bottom-pink 4s linear 2s infinite normal none running;       
   
}
.top-orange{
    background: linear-gradient( rgb(131, 9, 11), tomato);
    animation: top-orange 4s linear 2s infinite normal none running;
 }

.top-blue{
    animation: top-blue 4s linear 2s infinite normal none running;
 }

 form{
    margin-top: 40px;
    background-color: rgba(9, 149, 53, 0);
 }

 @keyframes passport{
    0%{
        border: 5px solid #cc2b5e;
    }
    25%{
        border: 5px solid rebeccapurple;
    }
    50%{
        border: 5px solid tomato;
    }
    75%{
        border: 5px solid yellow;
    }
    100%{
        border: 5px solid green;
    }
 }


@keyframes top-blue{
    0%{
        left: 50%;
        top: 7%;
        opacity: 1.0;        
    }
    20%{
        left: 50%;
        top: 30%;
        opacity: 0.7;        
    }
    40%{
        left: 50%;
        top: 50%;
        opacity: 0.4;
    }
    
    50%{
        left: 50%;
        top: 70%;
        opacity: 0.1;
    }

    60%{
        left: 50%;
        top: 50%;
        opacity: 0.4;
    }
    80%{
        left: 50%;
        top: 30%;
        opacity: 0.7;
    }
    100%{
        left: 50%;
        top: 5%;
        opacity: 1.0;
    }    
}

@keyframes bottom-pink{
   
}
</style>
