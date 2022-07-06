import {HubConnection, HubConnectionBuilder} from "@aspnet/signalr";
import {Predicate} from "@angular/core";
export class SignalR {
  private connection: HubConnection;

  constructor(url: string) {
    this.connection = new HubConnectionBuilder().withUrl(url).build();
  }

  register(methodname: string, method: Predicate<any>) {
    this.connection.on(methodname, method);
  }

  start() {
    this.connection.start().then( () => {
      console.log('SignalR connection started!');
    }).catch(err => {
      console.log('SignalR ERROR: ' + err.message)
    });
  }

}
