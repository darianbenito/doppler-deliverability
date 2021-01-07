pipeline {
    agent any
    stages {
        stage('build') {
            steps {
                sh 'docker build .' 
            }
        }
        stage('Publish in dopplerdock') {
            environment {
                DOCKER_CREDENTIALS_ID = "dockerhub_dopplerdock"
                DOCKER_IMAGE_NAME = "dopplerdock/doppler-deliverability"
            }
            stages {
                stage('Publish pre-release images from pull request') {
                    when {
                        changeRequest target: 'main'
                    }
                    steps {
                        withDockerRegistry(credentialsId: "${DOCKER_CREDENTIALS_ID}", url: "") {
                            sh 'sh build-n-publish.sh --image=${DOCKER_IMAGE_NAME} --commit=${GIT_COMMIT} --name=pr-${CHANGE_ID}'
                        }
                    }
                }
                stage('Publish pre-release images from main') {
                    when {
                        branch 'main'
                    }
                    steps {
                        withDockerRegistry(credentialsId: "${DOCKER_CREDENTIALS_ID}", url: "") {
                            sh 'sh build-n-publish.sh --image=${DOCKER_IMAGE_NAME} --commit=${GIT_COMMIT} --name=main'
                        }
                    }
                }
                stage('Publish pre-release images from INT') {
                    when {
                        branch 'INT'
                    }
                    steps {
                        withDockerRegistry(credentialsId: "${DOCKER_CREDENTIALS_ID}", url: "") {
                            sh 'sh build-n-publish.sh --image=${DOCKER_IMAGE_NAME} --commit=${GIT_COMMIT} --name=INT'
                        }
                    }
                }
                stage('Publish final version images') {
                    when {
                        expression {
                            return isVersionTag(readCurrentTag())
                        }
                    }
                    steps {
                        withDockerRegistry(credentialsId: "${DOCKER_CREDENTIALS_ID}", url: "") {
                            sh 'sh build-n-publish.sh --image=${DOCKER_IMAGE_NAME} --commit=${GIT_COMMIT} --version=${TAG_NAME}'
                        }
                    }
                }
            }
        }
    }
}

def boolean isVersionTag(String tag) {
    echo "checking version tag $tag"

    if (tag == null) {
        return false
    }

    // use your preferred pattern
    def tagMatcher = tag =~ /v\d+\.\d+\.\d+/

    return tagMatcher.matches()
}

def CHANGE_ID = env.CHANGE_ID

// https://stackoverflow.com/questions/56030364/buildingtag-always-returns-false
// workaround https://issues.jenkins-ci.org/browse/JENKINS-55987
// TODO: read this value from Jenkins provided metadata
def String readCurrentTag() {
    return sh(returnStdout: true, script: 'echo ${TAG_NAME}').trim()
}
