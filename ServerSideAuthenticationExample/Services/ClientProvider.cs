using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using ServerSideAuthenticationExample.Data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServerSideAuthenticationExample.Services
{
    public class ClientProvider
    {
        AuthenticationStateProvider auth_provider;
        TokenProvider token_provider;
        IJSRuntime js;
        NavigationManager nav;
        public AuthenticationState auth_state;
        HttpClient http;

        public bool is_logon => auth_state?.User?.Identity?.IsAuthenticated ?? false;

        public ClientProvider(AuthenticationStateProvider AuthProvider,
            TokenProvider token_provider,
            IJSRuntime js,
            NavigationManager nav,
            HttpClient http)
        {
            this.auth_provider = AuthProvider;
            this.token_provider = token_provider;
            this.js = js;
            this.nav = nav;
            this.http = http;
            load_auth_state();
        }

        private void load_auth_state()
        {
            auth_state = GetUser().Result;
        }


        private async Task<AuthenticationState> GetUser()
        {
            return await auth_provider.GetAuthenticationStateAsync();
        }

        /// <summary>
        /// 로그인
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pass"></param>
        /// <param name="keep_login"></param>
        /// <returns></returns>
        public async Task Login(string id, string pass, bool keep_login, string href = null)
        {
            try
            {
                //아디비번체크, 로그인 여부 판단 로직 작성영역


                //로그인 처리
                var param = JsonSerializer.Serialize(new SessionRequest
                {
                    session_id = id,
                    keep_login = keep_login
                });
                var stringContent = new StringContent(param, Encoding.UTF8, "application/json");
                stringContent.Headers.Add("__RequestVerificationToken", token_provider.AntiforgeryToken);

                var page_js = await js.InvokeAsync<IJSObjectReference>("import", $"/js/interop.js");
                string antiforgerytoken = token_provider.AntiforgeryToken;
                var fields = new { __RequestVerificationToken = antiforgerytoken, session_id = id, keep_login = keep_login };
                var receive = await page_js.InvokeAsync<string>("submitForm", "/login-request/", fields);

                nav.NavigateTo(href ?? nav.Uri, true);
            }
            catch (Exception ex)
            {

            }
        }

        public async Task Logout(string href = null)
        {
            try
            {
                string antiforgerytoken = token_provider.AntiforgeryToken;
                var fields = new { __RequestVerificationToken = antiforgerytoken };
                var page_js = await js.InvokeAsync<IJSObjectReference>("import", $"/js/interop.js");
                var receive = await page_js.InvokeAsync<string>("submitForm", "/logout-request/", fields);

                await Task.Delay(500);
                load_auth_state();



                nav.NavigateTo(href ?? nav.Uri, true);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
