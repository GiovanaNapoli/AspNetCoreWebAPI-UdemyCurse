import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { Util } from '../../../util/utils';
import { Disciplina } from 'src/app/model/disciplina';
import { Professor } from 'src/app/model/professor';

@Component({
  selector: 'app-professores-alunos',
  templateUrl: './professores-alunos.component.html',
  styleUrls: ['./professores-alunos.component.scss']
})
export class ProfessoresAlunosComponent implements OnInit {

  @Input() public professores: Professor[];
  @Output() closeModal = new EventEmitter();

  constructor(private router: Router) { }

  ngOnInit(): void {
  }

  disciplinaConcat(disciplinas: Disciplina[]): string {
    return Util.nomeConcat(disciplinas);
  }

  professorSelect(prof: Professor): void {
    this.closeModal.emit(null);
    this.router.navigate(['/professor', prof.id]);
  }

}
