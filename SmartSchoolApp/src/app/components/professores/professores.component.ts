import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { Disciplina } from 'src/app/model/disciplina';
import { Professor } from 'src/app/model/professor';
import { ProfessorService } from 'src/app/services/professor.service';
import { Util } from 'src/app/util/utils';

@Component({
  selector: 'app-professores',
  templateUrl: './professores.component.html',
  styleUrls: ['./professores.component.scss']
})
export class ProfessoresComponent implements OnInit, OnDestroy {

  public unsubscriber = new Subject();
  public professorSelecionado: Professor;
  public professores: Professor[];

  constructor(private router: Router,
    private spinner: NgxSpinnerService,
    private professorService: ProfessorService,
    private toastr: ToastrService) { }

  ngOnInit() {
    this.carregaProfessores();
  }
  ngOnDestroy(): void{
    this.unsubscriber.next();
    this.unsubscriber.complete();
  }

  carregaProfessores(){
    this.spinner.show();
    this.professorService.getAll()
      .pipe(takeUntil(this.unsubscriber))
      .subscribe((professores: Professor[]) => {
          this.professores = professores;
          this.toastr.success('Professores foram carregados com sucesso!');
          this.spinner.hide();
        }
      ), (error: any) => {
        this.toastr.error('Professores não carregados');
        console.error(error);
      }, () => this.spinner.hide();
  }

  disciplinaConcat(disciplinas: Disciplina[]){
    return Util.nomeConcat(disciplinas);
  }

}
