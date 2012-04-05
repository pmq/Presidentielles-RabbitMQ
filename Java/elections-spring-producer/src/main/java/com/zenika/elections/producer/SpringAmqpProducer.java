package com.zenika.elections.producer;

import org.springframework.amqp.core.AmqpTemplate;
import org.springframework.context.ApplicationContext;
import org.springframework.context.support.GenericXmlApplicationContext;

public class SpringAmqpProducer {

	private final static String MESSAGE = "<Dpt> <CodReg>82</CodReg> <CodReg3Car>082</CodReg3Car> <CodDpt>__DEPARTEMENT__</CodDpt>"
			+ "<CodDpt3Car>001</CodDpt3Car> <CodMinDpt>01</CodMinDpt> <LibDpt>AIN</LibDpt>"
			+ "<DateClotureDpt>19-07-2011</DateClotureDpt> <HeureClotureDpt>14:59:04</HeureClotureDpt>"
			+ "<Clos>CLOS</Clos> <Candidat> <Nom>SARKOZY</Nom> <Prenom>Nicolas</Prenom>"
			+ "<Civilite>M.</Civilite> <Voix>185165</Voix> <RapVoixExp>60,54</RapVoixExp>"
			+ "<NumDepCand>12</NumDepCand> <NumPanneauCand>1</NumPanneauCand> </Candidat> </Dpt>";

	public static void main(String[] args) throws InterruptedException {
		ApplicationContext context = new GenericXmlApplicationContext("classpath:/spring-rabbit.xml");
		AmqpTemplate template = context.getBean(AmqpTemplate.class);

		for (int i = 10; i < 97; i++) {
			String currResult = MESSAGE.replace("__DEPARTEMENT__", "" + i);
			template.convertAndSend(currResult);
			System.out.println("Published result for departement " + i + '.');

			Thread.sleep(3000);
		}
	}

}
